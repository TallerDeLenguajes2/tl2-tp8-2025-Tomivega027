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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Productos producto)
    {
        productoRepository.CrearProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = productoRepository.DetallesProducto(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(Productos producto)
    {
        productoRepository.ModificarProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = productoRepository.DetallesProducto(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult Delete(Productos producto)
    {
        productoRepository.EliminarProducto(producto.idProducto);
        return RedirectToAction("Index");
    }
}
