using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObligatorioApiario.Models
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El apiario es requerido")]
        public string NombreApiario { get; set; } = string.Empty;

        public string NivelPrioridad { get; set; } = "Baja";
        
        public string Icono { get; set; } = "fa-solid fa-briefcase-medical";

        public string Estado { get; set; } = "Programada"; // "Programada", "En Progreso", "Completada", "Cancelada"

        public string HerramientasRequeridas { get; set; } = string.Empty;

        public string NotasCampo { get; set; } = string.Empty;

        public string CreadoPor { get; set; } = "Admin (Sistema)";

        public string ClimaEstado { get; set; } = "Soleado";

        public string ClimaTemperatura { get; set; } = "22ºC";

        public string ColmenasRiesgo { get; set; } = "0/0";

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        public int? ColmenaId { get; set; }

        [ForeignKey("ColmenaId")]
        public Colmena? Colmena { get; set; }
    }
}
