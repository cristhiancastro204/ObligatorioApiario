using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ColmenasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColmenasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Colmenas/Create
        public IActionResult Create(int? apiarioId)
        {
            if (apiarioId == null)
            {
                return NotFound();
            }

            var apiario = _context.Apiarios.Find(apiarioId);
            if (apiario == null)
            {
                return NotFound();
            }

            ViewBag.ApiarioNombre = apiario.Nombre;
            
            var colmena = new Colmena
            {
                ApiarioId = apiarioId.Value,
                AnioReina = DateTime.Now.Year,
                FechaInstalacion = DateTime.Now,
                CantidadMarcos = 10,
                EstadoSalud = "Saludable"
            };

            return View(colmena);
        }

        // POST: Colmenas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApiarioId,TipoAbeja,CantidadAbejas,AnioReina,EstadoSalud,CantidadMarcos,FechaInstalacion,Notas")] Colmena colmena)
        {
            if (ModelState.IsValid)
            {
                // Obtener todas las colmenas para calcular el mÃ¡ximo Identificador actual
                var colmenasExistentes = await _context.Colmenas.ToListAsync();
                int maxNumero = 0;
                
                foreach (var c in colmenasExistentes)
                {
                    if (!string.IsNullOrEmpty(c.Identificador) && c.Identificador.StartsWith("COL-"))
                    {
                        var numeroString = c.Identificador.Substring(4);
                        if (int.TryParse(numeroString, out int numero) && numero > maxNumero)
                        {
                            maxNumero = numero;
                        }
                    }
                }
                
                colmena.Identificador = $"COL-{maxNumero + 1:000}";

                _context.Add(colmena);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Apiarios", new { id = colmena.ApiarioId });
            }
            
            var apiario = await _context.Apiarios.FindAsync(colmena.ApiarioId);
            ViewBag.ApiarioNombre = apiario?.Nombre;
            return View(colmena);
        }

        // GET: Colmenas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas
                .Include(c => c.Apiario)
                .Include(c => c.HistorialCosechas)
                    .ThenInclude(hc => hc.Cosecha)
                        .ThenInclude(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (colmena == null)
            {
                return NotFound();
            }

            return View(colmena);
        }

        // GET: Colmenas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas.FindAsync(id);
            if (colmena == null)
            {
                return NotFound();
            }
            
            var apiario = await _context.Apiarios.FindAsync(colmena.ApiarioId);
            ViewBag.ApiarioNombre = apiario?.Nombre;
            ViewBag.Apiarios = new SelectList(await _context.Apiarios.ToListAsync(), "Id", "Nombre", colmena.ApiarioId);

            return View(colmena);
        }

        // POST: Colmenas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApiarioId,Identificador,TipoAbeja,CantidadAbejas,AnioReina,EstadoSalud,CantidadMarcos,FechaInstalacion,Notas")] Colmena colmena)
        {
            if (id != colmena.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colmena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColmenaExists(colmena.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Apiarios", new { id = colmena.ApiarioId });
            }
            
            var apiario = await _context.Apiarios.FindAsync(colmena.ApiarioId);
            ViewBag.ApiarioNombre = apiario?.Nombre;
            return View(colmena);
        }

        private bool ColmenaExists(int id)
        {
            return _context.Colmenas.Any(e => e.Id == id);
        }

        // GET: Colmenas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colmena = await _context.Colmenas
                .Include(c => c.Apiario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colmena == null)
            {
                return NotFound();
            }

            return View(colmena);
        }

        // POST: Colmenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colmena = await _context.Colmenas.FindAsync(id);
            if (colmena != null)
            {
                var apiarioId = colmena.ApiarioId;
                _context.Colmenas.Remove(colmena);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Apiarios", new { id = apiarioId });
            }
            return RedirectToAction("Index", "Apiarios");
        }
    }
}

