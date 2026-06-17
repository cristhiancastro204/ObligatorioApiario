using System.ComponentModel.DataAnnotations;

namespace ObligatorioApiario.Models
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El apiario es requerido")]
        public string NombreApiario { get; set; } = string.Empty;

        public string NivelPrioridad { get; set; } = "Baja";

        public string Icono { get; set; } = "fa-solid fa-briefcase-medical";
    }
}
