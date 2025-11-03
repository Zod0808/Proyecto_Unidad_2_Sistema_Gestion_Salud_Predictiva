using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo para reportes de síntomas del paciente
    /// </summary>
    public class ReporteSintomas
    {
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public string NombrePaciente { get; set; }
        public DateTime FechaReporte { get; set; }
        public List<SintomaDetalle> Sintomas { get; set; }
        public string Ubicacion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string NotasAdicionales { get; set; }
        public int NivelUrgencia { get; set; } // 1-5
        public string RecomendacionIA { get; set; }
        public bool Procesado { get; set; }
        public string Estado { get; set; } // "Pendiente", "Revisado", "Atendido"

        public ReporteSintomas()
        {
            Id = Guid.NewGuid().ToString();
            FechaReporte = DateTime.Now;
            Sintomas = new List<SintomaDetalle>();
            Estado = "Pendiente";
            Procesado = false;
        }
    }

    public class SintomaDetalle
    {
        public string Nombre { get; set; }
        public string Gravedad { get; set; } // "Leve", "Moderado", "Grave"
        public int Duracion { get; set; } // En días
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// ViewModel para crear reporte de síntomas
    /// </summary>
    public class CrearReporteSintomasViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar al menos un síntoma")]
        [Display(Name = "Síntomas")]
        public List<string> SintomasSeleccionados { get; set; }

        [Display(Name = "Gravedad General")]
        public string Gravedad { get; set; }

        [Display(Name = "¿Desde hace cuántos días?")]
        [Range(0, 365, ErrorMessage = "Ingrese un valor válido")]
        public int Duracion { get; set; }

        [Display(Name = "Descripción detallada")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Ubicación actual")]
        public string Ubicacion { get; set; }

        [Display(Name = "Temperatura corporal (°C)")]
        [Range(35, 45, ErrorMessage = "Ingrese una temperatura válida")]
        public double? Temperatura { get; set; }

        public CrearReporteSintomasViewModel()
        {
            SintomasSeleccionados = new List<string>();
            Gravedad = "Leve";
            Duracion = 1;
        }
    }

    /// <summary>
    /// Lista de síntomas disponibles para reportar
    /// </summary>
    public static class SintomasDisponibles
    {
        public static readonly List<string> Respiratorios = new List<string>
        {
            "Tos seca",
            "Tos con flemas",
            "Dificultad para respirar",
            "Dolor de pecho",
            "Silbido al respirar",
            "Respiración rápida",
            "Falta de aire"
        };

        public static readonly List<string> Generales = new List<string>
        {
            "Fiebre",
            "Dolor de cabeza",
            "Dolor muscular",
            "Cansancio extremo",
            "Escalofríos",
            "Sudoración nocturna"
        };

        public static readonly List<string> NasalesGarganta = new List<string>
        {
            "Congestión nasal",
            "Dolor de garganta",
            "Estornudos frecuentes",
            "Pérdida del olfato",
            "Pérdida del gusto"
        };

        public static readonly List<string> Digestivos = new List<string>
        {
            "Náuseas",
            "Vómito",
            "Diarrea",
            "Dolor abdominal"
        };

        public static List<string> ObtenerTodos()
        {
            var todos = new List<string>();
            todos.AddRange(Respiratorios);
            todos.AddRange(Generales);
            todos.AddRange(NasalesGarganta);
            todos.AddRange(Digestivos);
            return todos;
        }
    }
}
