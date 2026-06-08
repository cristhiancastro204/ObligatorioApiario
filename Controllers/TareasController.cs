using Microsoft.AspNetCore.Mvc;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    public class TareasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TareaViewModel tareaVM)
        {
            var tarea = new Tarea
            {
                Titulo = tareaVM.Titulo,
                Descripcion = tareaVM.Descripcion,
                NombreApiario = tareaVM.NombreApiario,
                FechaVencimiento = tareaVM.FechaVencimiento == default ? DateTime.Now : tareaVM.FechaVencimiento,
                NivelPrioridad = string.IsNullOrEmpty(tareaVM.NivelPrioridad) ? "Baja" : tareaVM.NivelPrioridad,
                Icono = "fa-solid fa-circle-check"
            };

            _context.Tareas.Add(tarea);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Index(string ordenarPor = "prioridad")
        {
            var tareasDb = _context.Tareas.ToList();

            var tareasLista = tareasDb.Select(t => new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono
            }).ToList();

            // Lógica de ordenamiento
            if (ordenarPor == "fecha")
            {
                tareasLista = tareasLista.OrderBy(t => t.FechaVencimiento).ToList();
            }
            else // Por defecto, prioridad
            {
                tareasLista = tareasLista.OrderBy(t => t.NivelPrioridad == "Alta" ? 1 : t.NivelPrioridad == "Media" ? 2 : 3)
                               .ThenBy(t => t.FechaVencimiento)
                               .ToList();
            }

            ViewBag.OrdenActual = ordenarPor;

            return View(tareasLista);
        }

        public IActionResult Edit(int id)
        {
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            if (t == null) return NotFound();
            
            var tareaVM = new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono
            };
            return View(tareaVM);
        }

        [HttpPost]
        public IActionResult Edit(int id, TareaViewModel model)
        {
            var tareaExistente = _context.Tareas.FirstOrDefault(t => t.Id == id);
            if (tareaExistente != null)
            {
                tareaExistente.Titulo = model.Titulo;
                tareaExistente.Descripcion = model.Descripcion;
                tareaExistente.FechaVencimiento = model.FechaVencimiento;
                tareaExistente.NombreApiario = model.NombreApiario;
                tareaExistente.NivelPrioridad = model.NivelPrioridad;
                
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            if (t == null) return NotFound();
            
            var tareaVM = new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono
            };
            
            return View(tareaVM);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tarea = _context.Tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
