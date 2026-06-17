using Microsoft.AspNetCore.Mvc;
using ObligatorioApiario.Data;
using ObligatorioApiario.Models;
using System.Globalization;

namespace ObligatorioApiario.Controllers
{
    public class CalendarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? year, int? month)
        {
            var targetDate = DateTime.Now;

            // Ajustar si pasamos año y mes por parámetro
            if (year.HasValue && month.HasValue)
            {
                // Prevenir excepciones si los valores son raros
                try {
                    targetDate = new DateTime(year.Value, month.Value, 1);
                } catch {
                    targetDate = DateTime.Now;
                }
            }
            else 
            {
                targetDate = new DateTime(targetDate.Year, targetDate.Month, 1);
            }
            
            var viewModel = new CalendarViewModel
            {
                CurrentYear = targetDate.Year,
                CurrentMonth = targetDate.Month,
                PreviousYear = targetDate.AddMonths(-1).Year,
                PreviousMonth = targetDate.AddMonths(-1).Month,
                NextYear = targetDate.AddMonths(1).Year,
                NextMonth = targetDate.AddMonths(1).Month
            };

            // Nombre del mes en español
            TextInfo textInfo = new CultureInfo("es-ES", false).TextInfo;
            string monthName = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(targetDate.Month);
            viewModel.MonthName = textInfo.ToTitleCase(monthName.ToLower());

            // Calcular el inicio de la grilla (Lunes)
            var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // Domingo = 0, Lunes = 1...
            
            // Ajustar para que Lunes sea 0 y Domingo sea 6
            int daysToSubtract = firstDayOfWeek == 0 ? 6 : firstDayOfWeek - 1;
            var startDate = firstDayOfMonth.AddDays(-daysToSubtract);
            
            // Generar 42 celdas (6 semanas) para cubrir todos los meses
            for (int i = 0; i < 42; i++)
            {
                var currentDate = startDate.AddDays(i);
                var dayViewModel = new CalendarDayViewModel
                {
                    Date = currentDate,
                    IsOtherMonth = currentDate.Month != targetDate.Month
                };
                
                // Agregar dinámicamente las tareas programadas para este día desde la BD
                var tareasDelDia = _context.Tareas
                    .Where(t => t.FechaVencimiento.Date == currentDate.Date)
                    .ToList();

                foreach (var tarea in tareasDelDia)
                {
                    // Asignar color de evento según prioridad (Alta = rojo, Media/Baja = amarillo)
                    string typeClass = "yellow";
                    if (tarea.NivelPrioridad == "Alta") typeClass = "red";
                    
                    dayViewModel.Events.Add(new EventViewModel { 
                        Title = tarea.Titulo, 
                        Type = typeClass,
                        IconClass = tarea.NivelPrioridad == "Alta" ? "fa-solid fa-triangle-exclamation" : ""
                    });
                }

                viewModel.Days.Add(dayViewModel);
            }

            return View(viewModel);
        }
    }
}
