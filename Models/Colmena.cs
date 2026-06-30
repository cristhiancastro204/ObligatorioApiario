using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObligatorioApiario.Models
{
    public class Colmena
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El apiario es requerido")]
        public int ApiarioId { get; set; }

        [ForeignKey("ApiarioId")]
        public Apiario? Apiario { get; set; }

        [Display(Name = "Identificador (Automático)")]
        public string Identificador { get; set; } = string.Empty;

        [Display(Name = "Cantidad de Abejas Estimada")]
        public int CantidadAbejas { get; set; } = 50000;

        public ICollection<CosechaColmena> HistorialCosechas { get; set; } = new List<CosechaColmena>();

        [Display(Name = "Tipo de Abeja")]
        public string TipoAbeja { get; set; } = "Desconocida";

        [Display(Name = "Año de la Reina")]
        public int? AnioReina { get; set; }

        [Display(Name = "Estado de Salud")]
        public string EstadoSalud { get; set; } = "Saludable";

        [Display(Name = "Cantidad de Marcos")]
        public int CantidadMarcos { get; set; } = 10;

        [Display(Name = "Fecha de Instalación")]
        [DataType(DataType.Date)]
        public DateTime FechaInstalacion { get; set; } = DateTime.Now;

        [Display(Name = "Notas")]
        public string? Notas { get; set; }
    }
}
