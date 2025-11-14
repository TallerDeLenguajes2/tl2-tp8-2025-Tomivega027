using Microsoft.AspNetCore.Mvc;
using miproyecto.models;
using miproyecto.repository;
using System.Collections.Generic;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;

    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    // GET: /Productos/
    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productoRepository.GetAll();
        return View(productos);
    }
}
