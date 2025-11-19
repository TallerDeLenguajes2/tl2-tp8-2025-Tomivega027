using Microsoft.AspNetCore.Mvc;
using miproyecto.models;
using miproyecto.repository;
using System.Collections.Generic;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;
    private ProductoRepository productoRepository;

    public PresupuestosController()
    {
        presupuestosRepository = new PresupuestosRepository();
        productoRepository = new ProductoRepository();
    }

    //listamos todos los presupuesto de la DB
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presupuestosRepository.GetAll();
        return View(presupuestos);
    }

    //solo vista del formulario para crear presupuesto
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //trae los datos POST del formulario
    [HttpPost]
    public IActionResult Create(Presupuestos presupuesto)
    {
        presupuestosRepository.CrearPresupuesto(presupuesto);
        return RedirectToAction("Detalles", new { id = presupuesto.idPresupuestos });
    }

    //mostramos el presupuesto creado y un formulario POST para obtener detalles del presupuesto
    [HttpGet]
    public IActionResult Detalles(int id)
    {
        var presupuesto = presupuestosRepository.PresupuestoId(id);
        return View(presupuesto);
    }

    //Trae los datos del form POST y termina de crear el presupuesto
    [HttpPost]
    public IActionResult Detalles(int id, int idProducto, int cantidad)
    {
        presupuestosRepository.AgregarProducto(id, idProducto, cantidad);
        return RedirectToAction("Detalles", new { id = id });
    }

    //Nos deberia mostrar el presupuesto a editar
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestosRepository.PresupuestoId(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuestos presupuesto)
    {
        presupuestosRepository.Modificar(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = presupuestosRepository.PresupuestoId(id);
        
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult DeletePresupuesto(int id)
    {
        presupuestosRepository.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }
}

