namespace ObligatorioApiario.Models
{
    public class TareaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public string NombreApiario { get; set; } = string.Empty;
        public string NivelPrioridad { get; set; } = "Baja"; // "Alta", "Media", "Baja"
        public string Icono { get; set; } = "fa-solid fa-briefcase-medical";
        public string Estado { get; set; } = "Programada";
        public string HerramientasRequeridas { get; set; } = string.Empty;
        public string NotasCampo { get; set; } = string.Empty;
        public string CreadoPor { get; set; } = "Admin (Sistema)";
        public string ClimaEstado { get; set; } = "Soleado";
        public string ClimaTemperatura { get; set; } = "22ºC";
        public string ColmenasRiesgo { get; set; } = "0/0";
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public string? IdentificadorColmena { get; set; }
    }
}
