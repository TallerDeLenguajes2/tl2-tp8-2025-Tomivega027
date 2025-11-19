using Microsoft.Data.Sqlite;
using miproyecto.models;
using System.Collections.Generic;
namespace miproyecto.repository;

public class PresupuestosRepository
{
    private string cadenaConexion = "Data Source=Tienda.db";

    // ----------------------------------------------------
    // GET ALL
    // ----------------------------------------------------
    public List<Presupuestos> GetAll()
    {
        var lista = new List<Presupuestos>();

        string query = "SELECT * FROM presupuestos";

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        using var command = new SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            int id = Convert.ToInt32(reader ["idPresupuesto"]);

            var p = new Presupuestos
            {
                idPresupuestos = id,
                nombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
                fechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString())
            };
            p.detalles = PresupuestoId(id).detalles;

            lista.Add(p);
        }

        return lista;
    }

    // ----------------------------------------------------
    // CREAR PRESUPUESTO
    // ----------------------------------------------------
   public Presupuestos CrearPresupuesto(Presupuestos p)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        // INSERT presupuesto
        var command = new SqliteCommand(
            @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
              VALUES (@nombre, @fecha);
              SELECT last_insert_rowid();", connection);

        command.Parameters.AddWithValue("@nombre", p.nombreDestinatario);
        command.Parameters.AddWithValue("@fecha", p.fechaCreacion.ToString("yyyy-MM-dd HH:mm:ss"));

        p.idPresupuestos = Convert.ToInt32(command.ExecuteScalar());

        // INSERT detalles
        if (p.detalles != null)
        {
            foreach (var d in p.detalles)
            {
                var cmdDet = new SqliteCommand(
                    @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                      VALUES (@idPresupuesto, @idProducto, @cantidad)", connection);

                cmdDet.Parameters.AddWithValue("@idPresupuesto", p.idPresupuestos);
                cmdDet.Parameters.AddWithValue("@idProducto", d.producto.idProducto);
                cmdDet.Parameters.AddWithValue("@cantidad", d.cantidad);

                cmdDet.ExecuteNonQuery();
            }
        }

        return p;
    }

    // ----------------------------------------------------
    // OBTENER PRESUPUESTO POR ID + DETALLES
    // ----------------------------------------------------
    public Presupuestos PresupuestoId(int id)
    {
        var presupuesto = new Presupuestos();

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        // 1️⃣ PRESUPUESTO
        var cmd = new SqliteCommand(
            @"SELECT * FROM Presupuestos WHERE idPresupuesto = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);

        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                presupuesto.idPresupuestos = id;
                presupuesto.nombreDestinatario = reader["NombreDestinatario"].ToString();
                presupuesto.fechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString());
            }
        }

        // 2️⃣ DETALLES
        var cmdDet = new SqliteCommand(
            @"SELECT d.cantidad, p.idProducto, p.descripcion, p.precio
              FROM presupuestosDetalle d
              JOIN productos p ON p.idProducto = d.idProducto
              WHERE d.idPresupuesto = @id", connection);

        cmdDet.Parameters.AddWithValue("@id", id);

        using (var reader = cmdDet.ExecuteReader())
        {
            while (reader.Read())
            {
                presupuesto.detalles.Add(
                    new PresupuestosDetalles(
                        new Productos
                        {
                            idProducto = Convert.ToInt32(reader["idProducto"]),
                            descripcion = reader["descripcion"].ToString(),
                            precio = Convert.ToInt32(reader["precio"])
                        },
                        Convert.ToInt32(reader["cantidad"])
                    )
                );
            }
        }

        return presupuesto;
    }

    // ----------------------------------------------------
    // ELIMINAR PRESUPUESTO
    // ----------------------------------------------------
    public bool EliminarPresupuesto(int id)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        // Eliminar detalles primero
        var cmdDet = new SqliteCommand(
            "DELETE FROM presupuestosDetalle WHERE idPresupuesto = @id", connection);
        cmdDet.Parameters.AddWithValue("@id", id);
        cmdDet.ExecuteNonQuery();

        // Luego presupuesto
        var cmd = new SqliteCommand(
            "DELETE FROM presupuestos WHERE idPresupuesto = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);

        return cmd.ExecuteNonQuery() > 0;
    }

    // ----------------------------------------------------
    // AGREGAR PRODUCTO AL PRESUPUESTO
    // ----------------------------------------------------
    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        var cmd = new SqliteCommand(
            @"INSERT INTO presupuestosDetalle (idPresupuesto, idProducto, cantidad)
              VALUES (@idPresupuesto, @idProducto, @cantidad)",
            connection);

        cmd.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
        cmd.Parameters.AddWithValue("@idProducto", idProducto);
        cmd.Parameters.AddWithValue("@cantidad", cantidad);

        cmd.ExecuteNonQuery();
    }

         public void Modificar(Presupuestos presupuesto)
    {

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();
        string sql = "UPDATE Presupuestos SET NombreDestinatario=@NombreDestinatario,FechaCreacion=@fecha WHERE idPresupuesto = @id";
        using var command = new SqliteCommand(sql, connection);

        command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.nombreDestinatario));
        command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.fechaCreacion));
        command.Parameters.Add(new SqliteParameter("@id", presupuesto.idPresupuestos));

        command.ExecuteNonQuery();
        connection.Close();
    }
    public void AgregarProducto(int idPresupuesto, PresupuestosDetalles detalle)
    {
        // Verificamos que el objeto producto no sea nulo y tenga un ID válido.
        if (detalle.producto == null || detalle.producto.idProducto <= 0)
        {
            throw new ArgumentException("El producto proporcionado no es válido o no tiene un ID.");
        }

        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, cantidad) 
                     VALUES (@idPresupuesto, @idProducto, @cantidad);";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);

            // Aquí está la "traducción":
            // 1. Tomamos el idPresupuesto que viene como parámetro.
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            // 2. Extraemos el idProducto desde el objeto 'producto' dentro de 'detalle'.
            command.Parameters.AddWithValue("@idProducto", detalle.producto.idProducto);
            // 3. Tomamos la cantidad directamente de 'detalle'.
            command.Parameters.AddWithValue("@cantidad", detalle.cantidad);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    }
