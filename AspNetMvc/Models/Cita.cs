using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public enum EstadoCita
    {
        Programada = 1,
        Confirmada = 2,
        EnCurso = 3,
        Completada = 4,
        Cancelada = 5,
        NoAsistio = 6
    }

    public class Cita
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }

        public int MedicoId { get; set; }

        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        public DateTime FechaHora { get; set; }

        public EstadoCita Estado { get; set; }

        public string MotivoConsulta { get; set; }

        public string Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Cita()
        {
            Estado = EstadoCita.Programada;
            FechaCreacion = DateTime.Now;
        }

        public DateTime? FechaCancelacion { get; set; }

        public string MotivoCancelacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; }

        [ForeignKey("MedicoId")]
        public virtual Medico Medico { get; set; }

        [ForeignKey("EspecialidadId")]
        public virtual Especialidad Especialidad { get; set; }

        public virtual HistorialMedico HistorialMedico { get; set; }

        public string EstadoTexto
        {
            get
            {
                switch (Estado)
                {
                    case EstadoCita.Programada:
                        return "Programada";
                    case EstadoCita.Confirmada:
                        return "Confirmada";
                    case EstadoCita.EnCurso:
                        return "En Curso";
                    case EstadoCita.Completada:
                        return "Completada";
                    case EstadoCita.Cancelada:
                        return "Cancelada";
                    case EstadoCita.NoAsistio:
                        return "No Asistió";
                    default:
                        return "Desconocido";
                }
            }
        }
    }
}