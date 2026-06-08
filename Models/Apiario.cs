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
    }
}
