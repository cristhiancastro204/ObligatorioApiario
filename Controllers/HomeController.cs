using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Models;
using ObligatorioApiario.Data;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace ObligatorioApiario.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel();

        // Totales
        viewModel.TotalApiarios = await _context.Apiarios.CountAsync();
        viewModel.TotalColmenas = await _context.Colmenas.CountAsync();
        
        // Suma de Miel
        var todasLasCosechas = await _context.CosechasColmenas.ToListAsync();
        viewModel.TotalKilosMiel = todasLasCosechas.Sum(c => c.CantidadKg);

        // Tareas Pendientes (Programadas o En Progreso, ordenadas por fecha)
        viewModel.TareasPendientes = await _context.Tareas
            .Where(t => t.Estado == "Programada" || t.Estado == "En Progreso")
            .OrderBy(t => t.FechaVencimiento)
            .Take(5)
            .ToListAsync();

        // Datos para el gráfico: Agrupar cosechas por Mes/Año
        // Since sqlite/postgres grouping by dates can be tricky in EF, we'll fetch all or just the recent ones and group in memory
        var cosechasConFecha = await _context.Cosechas
            .Include(c => c.Detalles)
            .OrderBy(c => c.Fecha)
            .ToListAsync();

        var produccionAgrupada = cosechasConFecha
            .GroupBy(c => new { c.Fecha.Year, c.Fecha.Month })
            .Select(g => new ProduccionMensualData
            {
                Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yy", new CultureInfo("es-ES")).ToUpper(),
                TotalKg = g.SelectMany(c => c.Detalles).Sum(d => d.CantidadKg)
            })
            .ToList();

        // Si no hay datos, ponemos un par de meses vacíos para que el gráfico no se rompa
        if (!produccionAgrupada.Any())
        {
            produccionAgrupada.Add(new ProduccionMensualData { Mes = DateTime.Now.AddMonths(-1).ToString("MMM yy", new CultureInfo("es-ES")).ToUpper(), TotalKg = 0 });
            produccionAgrupada.Add(new ProduccionMensualData { Mes = DateTime.Now.ToString("MMM yy", new CultureInfo("es-ES")).ToUpper(), TotalKg = 0 });
        }

        viewModel.GraficoProduccion = produccionAgrupada;

        // Apiarios para el Mapa
        viewModel.MapaApiarios = await _context.Apiarios
            .Where(a => a.Latitud != 0 || a.Longitud != 0) // Solo los que tienen coordenadas
            .Select(a => new ApiarioUbicacionData
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Latitud = a.Latitud,
                Longitud = a.Longitud,
                Zona = a.Zona
            })
            .ToListAsync();

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

