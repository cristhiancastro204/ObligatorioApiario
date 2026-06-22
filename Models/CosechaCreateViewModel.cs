using System.ComponentModel.DataAnnotations;

namespace ObligatorioApiario.Models
{
    public class CosechaCreateViewModel
    {
        [Required]
        public int ApiarioId { get; set; }
        public string? ApiarioNombre { get; set; }

        [Required]
        [Display(Name = "Fecha de la Extracción")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now.Date;

        [Display(Name = "Notas Generales")]
        public string Notas { get; set; } = string.Empty;

        public List<CosechaColmenaInput> ColmenasInput { get; set; } = new();
    }

    public class CosechaColmenaInput
    {
        public int ColmenaId { get; set; }
        public string Identificador { get; set; } = string.Empty;
        public decimal CantidadKg { get; set; } = 0;
    }
}
