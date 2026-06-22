using System;
using System.Collections.Generic;

namespace ObligatorioApiario.Models
{
    public class CreateTareasViewModel
    {
        public string NombreApiario { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Programada";
        public string HerramientasRequeridas { get; set; } = string.Empty;
        public string ClimaEstado { get; set; } = "Soleado";
        public string ClimaTemperatura { get; set; } = "22ºC";
        public string ColmenasRiesgo { get; set; } = "0/0";

        // Lista de tareas específicas por cada colmena del apiario
        public List<TareaColmenaInputViewModel> TareasColmenas { get; set; } = new List<TareaColmenaInputViewModel>();
    }

    public class TareaColmenaInputViewModel
    {
        public int ColmenaId { get; set; }
        public string IdentificadorColmena { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string NotasCampo { get; set; } = string.Empty;
        public string NivelPrioridad { get; set; } = "Media";
    }
}
