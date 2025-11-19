using System.ComponentModel.DataAnnotations;
namespace miproyecto.models;
public class Productos
{
    public int idProducto { get; set; }
    
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string descripcion { get; set; }

    [Range(1, 999999, ErrorMessage = "El precio debe ser mayor que 0")]
    public double precio { get; set; }

    public Productos() { } // Constructor vacío necesario para ADO.NET o serialización JSON

    public Productos(int idProducto, string descripcion, int precio)
    {
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }
}