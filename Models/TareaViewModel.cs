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
    }
}
