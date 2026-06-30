using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;

namespace ObligatorioApiario.Controllers
{
    // Controlador responsable de manejar las solicitudes HTTP relacionadas con las Tareas.
    // Hereda de Controller, lo que le da funcionalidades para retornar vistas y manejar peticiones web.
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class TareasController : Controller
    {
        // Variable de solo lectura para interactuar con la base de datos a travÃ©s de Entity Framework Core.
        private readonly ApplicationDbContext _context;

        // Constructor del controlador. Inyecta el contexto de la base de datos (ApplicationDbContext) 
        // para que pueda ser utilizado en los diferentes mÃ©todos (acciones) del controlador.
        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // AcciÃ³n GET: Muestra el formulario para crear una nueva tarea.
        public IActionResult Create()
        {
            ViewBag.Apiarios = _context.Apiarios.ToList();
            return View(); // Retorna la vista asociada (Views/Tareas/Create.cshtml)
        }

        // AcciÃ³n POST: Recibe los datos del formulario para procesar y guardar las nuevas tareas por colmena.
        [HttpPost]
        public IActionResult Create(CreateTareasViewModel model)
        {
            if (model.TareasColmenas == null || !model.TareasColmenas.Any(tc => !string.IsNullOrWhiteSpace(tc.Titulo)))
            {
                // Si no se ingresÃ³ ninguna tarea, volvemos a la vista (se podrÃ­a agregar un mensaje de error)
                ViewBag.Apiarios = _context.Apiarios.ToList();
                return View();
            }

            foreach (var tc in model.TareasColmenas)
            {
                if (!string.IsNullOrWhiteSpace(tc.Titulo))
                {
                    var tarea = new Tarea
                    {
                        Titulo = tc.Titulo,
                        Descripcion = tc.Descripcion,
                        NombreApiario = model.NombreApiario,
                        FechaVencimiento = model.FechaVencimiento == default ? DateTime.Now : model.FechaVencimiento,
                        NivelPrioridad = string.IsNullOrEmpty(tc.NivelPrioridad) ? "Baja" : tc.NivelPrioridad,
                        Icono = "fa-solid fa-circle-check",
                        Estado = string.IsNullOrEmpty(model.Estado) ? "Programada" : model.Estado,
                        HerramientasRequeridas = model.HerramientasRequeridas ?? "",
                        NotasCampo = tc.NotasCampo ?? "",
                        CreadoPor = "Admin (Sistema)",
                        ClimaEstado = string.IsNullOrEmpty(model.ClimaEstado) ? "Soleado" : model.ClimaEstado,
                        ClimaTemperatura = string.IsNullOrEmpty(model.ClimaTemperatura) ? "22ÂºC" : model.ClimaTemperatura,
                        ColmenasRiesgo = string.IsNullOrEmpty(model.ColmenasRiesgo) ? "0/0" : model.ColmenasRiesgo,
                        FechaActualizacion = DateTime.Now,
                        ColmenaId = tc.ColmenaId
                    };
                    _context.Tareas.Add(tarea);
                }
            }

            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetColmenasPorApiario(string nombreApiario)
        {
            var apiario = _context.Apiarios
                .Include(a => a.Colmenas)
                .FirstOrDefault(a => a.Nombre == nombreApiario);

            if (apiario == null) return Json(new List<object>());

            var colmenas = apiario.Colmenas.Select(c => new {
                id = c.Id,
                identificador = c.Identificador
            }).ToList();

            return Json(colmenas);
        }
        
        // AcciÃ³n GET: Muestra la lista de tareas. 
        // Permite recibir un parÃ¡metro 'ordenarPor' para decidir cÃ³mo listar los datos (por defecto ordena por "prioridad").
        public IActionResult Index(string ordenarPor = "prioridad")
        {
            // Obtiene todas las tareas de la base de datos, incluyendo la informaciÃ³n de la Colmena asociada.
            var tareasDb = _context.Tareas.Include(t => t.Colmena).ToList();

            // Mapea la lista de modelos de dominio (Tarea) a una lista de modelos de vista (TareaViewModel)
            // para enviar solo la informaciÃ³n necesaria a la interfaz.
            var tareasLista = tareasDb.Select(t => new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono,
                Estado = t.Estado,
                HerramientasRequeridas = t.HerramientasRequeridas,
                NotasCampo = t.NotasCampo,
                CreadoPor = t.CreadoPor,
                ClimaEstado = t.ClimaEstado,
                ClimaTemperatura = t.ClimaTemperatura,
                ColmenasRiesgo = t.ColmenasRiesgo,
                FechaActualizacion = t.FechaActualizacion,
                IdentificadorColmena = t.Colmena?.Identificador
            }).ToList();

            // LÃ³gica de ordenamiento segÃºn la preferencia del usuario
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

        // AcciÃ³n GET: Muestra los detalles de una tarea especÃ­fica.
        public IActionResult Details(int id)
        {
            // Busca la tarea por su ID.
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            
            // Si no se encuentra la tarea, retorna 404 Not Found.
            if (t == null) return NotFound();

            // Mapea la tarea a un ViewModel para enviarlo a la vista de detalles.
            var tareaVM = new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono,
                Estado = t.Estado,
                HerramientasRequeridas = t.HerramientasRequeridas,
                NotasCampo = t.NotasCampo,
                CreadoPor = t.CreadoPor,
                ClimaEstado = t.ClimaEstado,
                ClimaTemperatura = t.ClimaTemperatura,
                ColmenasRiesgo = t.ColmenasRiesgo,
                FechaActualizacion = t.FechaActualizacion
            };

            return View(tareaVM); // Retorna la vista Views/Tareas/Details.cshtml
        }

        // AcciÃ³n GET: Muestra el formulario para editar una tarea existente.
        // Recibe el 'id' de la tarea a editar.
        public IActionResult Edit(int id)
        {
            // Busca la primera tarea que coincida con el ID proporcionado.
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            
            // Si no se encuentra la tarea, retorna un error HTTP 404 (Not Found).
            if (t == null) return NotFound();
            
            ViewBag.Apiarios = _context.Apiarios.ToList();

            // Mapea la tarea encontrada a un ViewModel para enviarlo a la vista de ediciÃ³n.
            var tareaVM = new TareaViewModel
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                NombreApiario = t.NombreApiario,
                FechaVencimiento = t.FechaVencimiento,
                NivelPrioridad = t.NivelPrioridad,
                Icono = t.Icono,
                Estado = t.Estado,
                HerramientasRequeridas = t.HerramientasRequeridas,
                NotasCampo = t.NotasCampo,
                CreadoPor = t.CreadoPor,
                ClimaEstado = t.ClimaEstado,
                ClimaTemperatura = t.ClimaTemperatura,
                ColmenasRiesgo = t.ColmenasRiesgo,
                FechaActualizacion = t.FechaActualizacion
            };
            return View(tareaVM); // Retorna la vista Views/Tareas/Edit.cshtml
        }

        // AcciÃ³n POST: Recibe los datos actualizados del formulario de ediciÃ³n y guarda los cambios.
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
                tareaExistente.Estado = model.Estado;
                tareaExistente.HerramientasRequeridas = model.HerramientasRequeridas ?? "";
                tareaExistente.NotasCampo = model.NotasCampo ?? "";
                tareaExistente.ClimaEstado = model.ClimaEstado;
                tareaExistente.ClimaTemperatura = model.ClimaTemperatura;
                tareaExistente.ColmenasRiesgo = model.ColmenasRiesgo;
                tareaExistente.FechaActualizacion = DateTime.Now;
                
                // Guarda los cambios aplicados en la base de datos.
                _context.SaveChanges();
            }
            
            // Redirige al usuario nuevamente a la lista de tareas tras guardar los cambios.
            return RedirectToAction(nameof(Index));
        }

        // AcciÃ³n GET: Muestra una vista de confirmaciÃ³n para eliminar una tarea.
        // Recibe el 'id' de la tarea que se desea eliminar.
        public IActionResult Delete(int id)
        {
            // Busca la tarea en la base de datos.
            var t = _context.Tareas.FirstOrDefault(x => x.Id == id);
            
            // Si la tarea no existe, retorna 404 Not Found.
            if (t == null) return NotFound();
            
            // Pasa la informaciÃ³n a un ViewModel para mostrar los detalles al usuario antes de confirmar la eliminaciÃ³n.
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

        // AcciÃ³n POST: Confirma y realiza la eliminaciÃ³n de la tarea de la base de datos.
        // Se utiliza el atributo ActionName("Delete") porque el mÃ©todo no puede llamarse Delete igual que el GET al tener los mismos parÃ¡metros.
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

