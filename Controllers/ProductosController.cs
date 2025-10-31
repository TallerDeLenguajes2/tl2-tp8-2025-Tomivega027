public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

PUBLIC

    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

}
//Ejemplo de cómo “Listar” los producto desde Index
[HttpGet]
public IActionResult Index()
{
    List<Producto> productos = productoRepository.GetAll();
    return View(productos);
}
