using Microsoft.AspNetCore.Mvc;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    // Controlador responsable de manejar las solicitudes HTTP relacionadas con las Tareas.
    // Hereda de Controller, lo que le da funcionalidades para retornar vistas y manejar peticiones web.
    public class TareasController : Controller
    {
        // Variable de solo lectura para interactuar con la base de datos a través de Entity Framework Core.
        private readonly ApplicationDbContext _context;

        // Constructor del controlador. Inyecta el contexto de la base de datos (ApplicationDbContext) 
        // para que pueda ser utilizado en los diferentes métodos (acciones) del controlador.
        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET: Muestra el formulario para crear una nueva tarea.
        public IActionResult Create()
        {
            return View(); // Retorna la vista asociada (Views/Tareas/Create.cshtml)
        }

        // Acción POST: Recibe los datos del formulario para procesar y guardar la nueva tarea.
        // Se ejecuta cuando el usuario envía el formulario.
        [HttpPost]
        public IActionResult Create(TareaViewModel tareaVM)
        {
            // Mapea los datos recibidos de la vista (ViewModel) al modelo de dominio (Tarea)
            var tarea = new Tarea
            {
                Titulo = tareaVM.Titulo,
                Descripcion = tareaVM.Descripcion,
                NombreApiario = tareaVM.NombreApiario,
                // Si la fecha por defecto viene vacía, le asigna la fecha actual, sino usa la proporcionada.
                FechaVencimiento = tareaVM.FechaVencimiento == default ? DateTime.Now : tareaVM.FechaVencimiento,
                // Si la prioridad viene nula o vacía, le asigna "Baja" por defecto.
                NivelPrioridad = string.IsNullOrEmpty(tareaVM.NivelPrioridad) ? "Baja" : tareaVM.NivelPrioridad,
                // Define un ícono fijo por defecto para la tarea.
                Icono = "fa-solid fa-circle-check"
            };

            // Agrega la nueva tarea al contexto y guarda los cambios en la base de datos.
            _context.Tareas.Add(tarea);
            _context.SaveChanges();
            
            // Redirige al usuario a la lista de tareas (acción Index) una vez guardada.
            return RedirectToAction(nameof(Index));
        }
        
        // Acción GET: Muestra la lista de tareas. 
        // Permite recibir un parámetro 'ordenarPor' para decidir cómo listar los datos (por defecto ordena por "prioridad").
        public IActionResult Index(string ordenarPor = "prioridad")
        {
            // Obtiene todas las tareas de la base de datos.
            var tareasDb = _context.Tareas.ToList();

            // Mapea la lista de modelos de dominio (Tarea) a una lista de modelos de vista (TareaViewModel)
            // para enviar solo la información necesaria a la interfaz.
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

            // Lógica de ordenamiento según la preferencia del usuario
            if (ordenarPor == "fecha")
            {
                // Ordena la lista de tareas por fecha de vencimiento ascendente.
                tareasLista = tareasLista.OrderBy(t => t.FechaVencimiento).ToList();
            }
            else // Por defecto, ordena por prioridad
            {
                // Ordena primero por nivel de prioridad (Alta primero, luego Media, luego Baja)
                // y en caso de empate, ordena por fecha de vencimiento.
                tareasLista = tareasLista.OrderBy(t => t.NivelPrioridad == "Alta" ? 1 : t.NivelPrioridad == "Media" ? 2 : 3)
                               .ThenBy(t => t.FechaVencimiento)
                               .ToList();
            }

            // Guarda el orden actual en el ViewBag para mantener la referencia en la vista (por ej., para pintar botones).
            ViewBag.OrdenActual = ordenarPor;

            // Retorna la vista pasando la lista ordenada de tareas.
            return View(tareasLista);
        }

        // Acción GET: Muestra el formulario para editar una tarea existente.
        // Recibe el 'id' de la tarea a editar.
        public IActionResult Edit(int id)
        {
            // Busca la primera tarea que coincida con el ID proporcionado.
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            
            // Si no se encuentra la tarea, retorna un error HTTP 404 (Not Found).
            if (t == null) return NotFound();
            
            // Mapea la tarea encontrada a un ViewModel para enviarlo a la vista de edición.
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
            return View(tareaVM); // Retorna la vista Views/Tareas/Edit.cshtml
        }

        // Acción POST: Recibe los datos actualizados del formulario de edición y guarda los cambios.
        [HttpPost]
        public IActionResult Edit(int id, TareaViewModel model)
        {
            // Busca la tarea existente en la base de datos por su ID.
            var tareaExistente = _context.Tareas.FirstOrDefault(t => t.Id == id);
            
            if (tareaExistente != null)
            {
                // Actualiza las propiedades de la tarea existente con los datos recibidos del formulario (model).
                tareaExistente.Titulo = model.Titulo;
                tareaExistente.Descripcion = model.Descripcion;
                tareaExistente.FechaVencimiento = model.FechaVencimiento;
                tareaExistente.NombreApiario = model.NombreApiario;
                tareaExistente.NivelPrioridad = model.NivelPrioridad;
                
                // Guarda los cambios aplicados en la base de datos.
                _context.SaveChanges();
            }
            
            // Redirige al usuario nuevamente a la lista de tareas tras guardar los cambios.
            return RedirectToAction(nameof(Index));
        }

        // Acción GET: Muestra una vista de confirmación para eliminar una tarea.
        // Recibe el 'id' de la tarea que se desea eliminar.
        public IActionResult Delete(int id)
        {
            // Busca la tarea en la base de datos.
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            
            // Si la tarea no existe, retorna 404 Not Found.
            if (t == null) return NotFound();
            
            // Pasa la información a un ViewModel para mostrar los detalles al usuario antes de confirmar la eliminación.
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
            
            return View(tareaVM); // Retorna la vista Views/Tareas/Delete.cshtml
        }

        // Acción POST: Confirma y realiza la eliminación de la tarea de la base de datos.
        // Se utiliza el atributo ActionName("Delete") porque el método no puede llamarse Delete igual que el GET al tener los mismos parámetros.
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Busca la tarea por su ID.
            var tarea = _context.Tareas.FirstOrDefault(t => t.Id == id);
            
            if (tarea != null)
            {
                // Remueve la entidad del contexto y aplica los cambios para borrarla en la BD.
                _context.Tareas.Remove(tarea);
                _context.SaveChanges();
            }
            
            // Vuelve al listado principal una vez eliminada.
            return RedirectToAction(nameof(Index));
        }
    }
}
