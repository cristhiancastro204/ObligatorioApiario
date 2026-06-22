using System.ComponentModel.DataAnnotations;

namespace ObligatorioApiario.Models
{
    public class Apiario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;

        public string? Notas { get; set; }

        public string Zona { get; set; } = string.Empty;

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        // Relación 1 a muchos: Un apiario puede tener varias tareas, colmenas y cosechas
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
        public ICollection<Colmena> Colmenas { get; set; } = new List<Colmena>();
        public ICollection<Cosecha> Cosechas { get; set; } = new List<Cosecha>();
    }
}
