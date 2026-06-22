using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObligatorioApiario.Models
{
    public class CosechaColmena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CosechaId { get; set; }

        [ForeignKey("CosechaId")]
        public Cosecha? Cosecha { get; set; }

        [Required]
        public int ColmenaId { get; set; }

        [ForeignKey("ColmenaId")]
        public Colmena? Colmena { get; set; }

        [Required]
        [Display(Name = "Kilos Producidos")]
        public decimal CantidadKg { get; set; } = 0;
    }
}
