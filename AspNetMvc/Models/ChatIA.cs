using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class ChatIA
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string ResumenTriaje { get; set; }

        public string SintomasReportados { get; set; }

        public string EvaluacionIA { get; set; }

        public string RecomendacionesIA { get; set; }

        public int? NivelUrgencia { get; set; } // 1-5, siendo 5 urgente

        public bool Completado { get; set; }

        // Propiedades de navegación
        [ForeignKey("PacienteId")]
        public virtual Paciente Paciente { get; set; }

        public virtual ICollection<MensajeChatIA> Mensajes { get; set; }

        public ChatIA()
        {
            FechaInicio = DateTime.Now;
            Completado = false;
            Mensajes = new List<MensajeChatIA>();
        }

        public string NivelUrgenciaTexto
        {
            get
            {
                switch (NivelUrgencia)
                {
                    case 1:
                        return "Muy Bajo";
                    case 2:
                        return "Bajo";
                    case 3:
                        return "Moderado";
                    case 4:
                        return "Alto";
                    case 5:
                        return "Urgente";
                    default:
                        return "No Evaluado";
                }
            }
        }
    }

    public class MensajeChatIA
    {
        public int Id { get; set; }

        public int ChatIAId { get; set; }

        public DateTime Timestamp { get; set; }

        public bool EsDelPaciente { get; set; }

        public string Mensaje { get; set; }

        public string TipoPregunta { get; set; } // sintoma, historial, examen, etc.

        // Propiedades de navegación
        [ForeignKey("ChatIAId")]
        public virtual ChatIA ChatIA { get; set; }

        public MensajeChatIA()
        {
            Timestamp = DateTime.Now;
        }
    }
}