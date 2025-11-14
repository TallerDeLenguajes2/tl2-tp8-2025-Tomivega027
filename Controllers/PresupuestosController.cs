using Microsoft.AspNetCore.Mvc;
using miproyecto.models;
using miproyecto.repository;
using System.Collections.Generic;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;

    public PresupuestosController()
    {
        presupuestosRepository = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> lista = presupuestosRepository.GetAll();
        return View(lista);
    }
}
