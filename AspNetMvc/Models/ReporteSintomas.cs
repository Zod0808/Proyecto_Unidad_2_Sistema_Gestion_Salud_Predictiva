using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Reporte de Síntomas
    /// </summary>
    public class ReporteSintomas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        [Display(Name = "ID del Usuario")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "El nombre del paciente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Paciente")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150 años")]
        [Display(Name = "Edad")]
        public int Age { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [Display(Name = "Género")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Debe tener al menos un síntoma")]
        [Display(Name = "Síntomas")]
        public List<string> Symptoms { get; set; } = new List<string>();

        [StringLength(1000, ErrorMessage = "Las notas adicionales no pueden exceder 1000 caracteres")]
        [Display(Name = "Notas Adicionales")]
        public string AdditionalNotes { get; set; }

        [Display(Name = "Ubicación")]
        public Ubicacion Location { get; set; }

        [Display(Name = "Teléfono de Contacto")]
        [Phone(ErrorMessage = "Ingresa un número de teléfono válido")]
        public string ContactPhone { get; set; }

        [Display(Name = "Email de Contacto")]
        [EmailAddress(ErrorMessage = "Ingresa un email válido")]
        public string ContactEmail { get; set; }

        [Display(Name = "Resultado del Análisis IA")]
        public AnalisisIA AIAnalysis { get; set; }

        [Required]
        [Display(Name = "Fecha de Reporte")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReportedAt { get; set; } = DateTime.Now;

        [Display(Name = "Estado")]
        [BsonRepresentation(BsonType.String)]
        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        [Display(Name = "Fecha de Creación")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Actualización")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Propiedades calculadas
        [BsonIgnore]
        public int SymptomsCount => Symptoms?.Count ?? 0;

        [BsonIgnore]
        public bool HasLocation => Location != null && Location.HasCoordinates;

        [BsonIgnore]
        public bool HasAIAnalysis => AIAnalysis != null;

        [BsonIgnore]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case ReportStatus.Pending:
                        return "Pendiente";
                    case ReportStatus.InReview:
                        return "En Revisión";
                    case ReportStatus.Reviewed:
                        return "Revisado";
                    case ReportStatus.Closed:
                        return "Cerrado";
                    default:
                        return "Desconocido";
                }
            }
        }

        [BsonIgnore]
        public bool IsUrgent => HasAIAnalysis && AIAnalysis.RequiresImmediateAttention;
    }

    /// <summary>
    /// Estados de reporte
    /// </summary>
    public enum ReportStatus
    {
        Pending,   // Pendiente
        InReview,  // En revisión
        Reviewed,  // Revisado
        Closed     // Cerrado
    }
}

