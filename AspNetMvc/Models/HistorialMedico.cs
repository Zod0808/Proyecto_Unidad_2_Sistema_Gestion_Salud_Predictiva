using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class HistorialMedico
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }

        public int MedicoId { get; set; }

        public int? CitaId { get; set; }

        public DateTime Fecha { get; set; }

        public string Diagnostico { get; set; }

        public string Sintomas { get; set; }

        public string Tratamiento { get; set; }

        public string Medicamentos { get; set; }

        public string Observaciones { get; set; }

        public string SignosVitales { get; set; } // JSON con presión, temperatura, etc.

        public string ExamenesRealizados { get; set; }

        public string ResultadosExamenes { get; set; }

        public string Recomendaciones { get; set; }

        public DateTime? ProximaRevision { get; set; }

        // Propiedades de navegación
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; }

        [ForeignKey("MedicoId")]
        public virtual Medico Medico { get; set; }

        [ForeignKey("CitaId")]
        public virtual Cita Cita { get; set; }

        public HistorialMedico()
        {
            Fecha = DateTime.Now;
        }
    }
}