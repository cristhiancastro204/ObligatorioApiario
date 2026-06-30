using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ApiariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var apiarios = _context.Apiarios
                .Include(a => a.Colmenas)
                .Include(a => a.Cosechas)
                .ThenInclude(c => c.Detalles)
                .ToList();
            return View(apiarios);
        }

        // GET: Apiarios/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = _context.Apiarios.Include(a => a.Colmenas).FirstOrDefault(m => m.Id == id);
            if (apiario == null)
            {
                return NotFound();
            }

            return View(apiario);
        }

        // GET: Apiarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apiarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Apiario apiario)
        {
            if (ModelState.IsValid)
            {
                _context.Apiarios.Add(apiario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(apiario);
        }
        // GET: Apiarios/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = _context.Apiarios.Find(id);
            if (apiario == null)
            {
                return NotFound();
            }
            return View(apiario);
        }

        // POST: Apiarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Apiario apiario)
        {
            if (id != apiario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(apiario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(apiario);
        }

        // GET: Apiarios/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apiario = _context.Apiarios.FirstOrDefault(m => m.Id == id);
            if (apiario == null)
            {
                return NotFound();
            }

            return View(apiario);
        }

        // POST: Apiarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var apiario = _context.Apiarios.Find(id);
            if (apiario != null)
            {
                _context.Apiarios.Remove(apiario);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

