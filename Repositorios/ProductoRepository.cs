using Microsoft.Data.Sqlite;
using miproyecto.models;
using System;
using System.Collections.Generic;
namespace miproyecto.repository;
public class ProductoRepository
{
    private string cadenaConexion = "Data Source=Tienda.db";

    // -------------------------------------------------------
    // GET ALL
    // -------------------------------------------------------
    public List<Productos> GetAll()
    {
        var productos = new List<Productos>();

        string query = "SELECT * FROM Productos";

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        using var command = new SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            productos.Add(new Productos
            {
                idProducto = Convert.ToInt32(reader["idProducto"]),
                descripcion = reader["descripcion"].ToString(),
                precio = Convert.ToDouble(reader["precio"])
            });
        }

        return productos;
    }

    // -------------------------------------------------------
    // CREATE
    // -------------------------------------------------------
    public Productos CrearProducto(Productos prod)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        var command = new SqliteCommand(
            @"INSERT INTO Productos (descripcion, precio) 
              VALUES (@descripcion, @precio); 
              SELECT last_insert_rowid();",
            connection);

        command.Parameters.AddWithValue("@descripcion", prod.descripcion);
        command.Parameters.AddWithValue("@precio", prod.precio);

        prod.idProducto = Convert.ToInt32(command.ExecuteScalar());
        return prod;
    }

    // -------------------------------------------------------
    // UPDATE
    // -------------------------------------------------------
    public Productos ModificarProducto(int id, Productos prod)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        var command = new SqliteCommand(
            @"UPDATE Productos 
              SET descripcion = @descripcion, precio = @precio 
              WHERE idProducto = @id",
            connection);

        command.Parameters.AddWithValue("@descripcion", prod.descripcion);
        command.Parameters.AddWithValue("@precio", prod.precio);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
        return prod;
    }

    // -------------------------------------------------------
    // GET BY ID
    // -------------------------------------------------------
    public Productos DetallesProducto(int id)
    {
        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        var command = new SqliteCommand(
            "SELECT * FROM Productos WHERE idProducto = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Productos
            {
                idProducto = Convert.ToInt32(reader["idProducto"]),
                descripcion = reader["descripcion"].ToString(),
                precio = Convert.ToInt32(reader["precio"])
            };
        }

        return null;
    }

    // -------------------------------------------------------
    // DELETE
    // -------------------------------------------------------
    public Productos EliminarProducto(int id)
    {
        Productos eliminado = DetallesProducto(id);
        if (eliminado == null)
            return null;

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        var cmd = new SqliteCommand("DELETE FROM Productos WHERE idProducto = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        return eliminado;
    }
}
