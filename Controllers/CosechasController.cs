using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CosechasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CosechasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cosechas/Create?apiarioId=5
        public async Task<IActionResult> Create(int apiarioId)
        {
            var apiario = await _context.Apiarios
                .Include(a => a.Colmenas)
                .FirstOrDefaultAsync(a => a.Id == apiarioId);

            if (apiario == null)
                return NotFound();

            var viewModel = new CosechaCreateViewModel
            {
                ApiarioId = apiarioId,
                ApiarioNombre = apiario.Nombre,
                ColmenasInput = apiario.Colmenas.Select(c => new CosechaColmenaInput
                {
                    ColmenaId = c.Id,
                    Identificador = c.Identificador,
                    CantidadKg = 0
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Cosechas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CosechaCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cosecha = new Cosecha
                {
                    ApiarioId = model.ApiarioId,
                    Fecha = model.Fecha,
                    Notas = model.Notas
                };

                _context.Cosechas.Add(cosecha);
                await _context.SaveChangesAsync();

                foreach (var input in model.ColmenasInput)
                {
                    var detalle = new CosechaColmena
                    {
                        CosechaId = cosecha.Id,
                        ColmenaId = input.ColmenaId,
                        CantidadKg = input.CantidadKg
                    };
                    _context.CosechasColmenas.Add(detalle);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Apiarios", new { id = model.ApiarioId });
            }

            return View(model);
        }
    }
}

