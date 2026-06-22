using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObligatorioApiario.Models
{
    public class Cosecha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApiarioId { get; set; }
        
        [ForeignKey("ApiarioId")]
        public Apiario? Apiario { get; set; }

        [Required]
        [Display(Name = "Fecha de Cosecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Notas Generales")]
        public string Notas { get; set; } = string.Empty;

        public ICollection<CosechaColmena> Detalles { get; set; } = new List<CosechaColmena>();
    }
}
