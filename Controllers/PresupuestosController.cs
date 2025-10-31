using miproyecto.
public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository();
    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestosRepository();
    }
}

[HttpGet]
public IActionResult Index()
{
    List<Presupuestos> presupuestos = presupuestosRepository.GetAll();
    return View(presupuestos);
}
