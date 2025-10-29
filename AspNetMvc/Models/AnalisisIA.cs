using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Análisis de IA
    /// </summary>
    public class AnalisisIA
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El ID de la historia médica es obligatorio")]
        [Display(Name = "ID de Historia Médica")]
        public string MedicalHistoryId { get; set; }

        [Required(ErrorMessage = "Los síntomas son obligatorios")]
        [Display(Name = "Síntomas")]
        public List<Sintoma> Symptoms { get; set; } = new List<Sintoma>();

        [Required(ErrorMessage = "Los diagnósticos posibles son obligatorios")]
        [Display(Name = "Diagnósticos Posibles")]
        public List<DiagnosticoPosible> PossibleDiagnoses { get; set; } = new List<DiagnosticoPosible>();

        [Required(ErrorMessage = "La urgencia es obligatoria")]
        [Display(Name = "Urgencia")]
        [BsonRepresentation(BsonType.String)]
        public UrgencyLevel Urgency { get; set; }

        [Required(ErrorMessage = "La confianza es obligatoria")]
        [Range(0, 100, ErrorMessage = "La confianza debe estar entre 0 y 100")]
        [Display(Name = "Confianza (%)")]
        public double Confidence { get; set; }

        [Required]
        [Display(Name = "Fecha y Hora")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Creación")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Actualización")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Propiedades calculadas
        [BsonIgnore]
        public string UrgencyText
        {
            get
            {
                switch (Urgency)
                {
                    case UrgencyLevel.Low:
                        return "Baja";
                    case UrgencyLevel.Medium:
                        return "Media";
                    case UrgencyLevel.High:
                        return "Alta";
                    case UrgencyLevel.Critical:
                        return "Crítica";
                    default:
                        return "Desconocida";
                }
            }
        }

        [BsonIgnore]
        public string ConfidenceText
        {
            get
            {
                if (Confidence >= 90) return "Muy Alta";
                if (Confidence >= 70) return "Alta";
                if (Confidence >= 50) return "Media";
                if (Confidence >= 30) return "Baja";
                return "Muy Baja";
            }
        }

        [BsonIgnore]
        public DiagnosticoPosible TopDiagnosis
        {
            get
            {
                if (PossibleDiagnoses == null || PossibleDiagnoses.Count == 0)
                    return null;

                return PossibleDiagnoses.OrderByDescending(d => d.Probability).FirstOrDefault();
            }
        }

        [BsonIgnore]
        public bool IsCritical => Urgency == UrgencyLevel.Critical;

        [BsonIgnore]
        public bool RequiresImmediateAttention => Urgency == UrgencyLevel.Critical || Urgency == UrgencyLevel.High;

        [BsonIgnore]
        public bool HasHighConfidence => Confidence >= 70;

        [BsonIgnore]
        public int SymptomsCount => Symptoms?.Count ?? 0;

        [BsonIgnore]
        public int DiagnosesCount => PossibleDiagnoses?.Count ?? 0;

        // Métodos de negocio
        public bool IsValid()
        {
            return Symptoms != null && Symptoms.Count > 0 && Symptoms.Count <= 50 &&
                   PossibleDiagnoses != null && PossibleDiagnoses.Count > 0 && PossibleDiagnoses.Count <= 10 &&
                   Confidence >= 0 && Confidence <= 100;
        }
    }

    /// <summary>
    /// Modelo de Diagnóstico Posible
    /// </summary>
    public class DiagnosticoPosible
    {
        [Required(ErrorMessage = "El nombre de la condición es obligatorio")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        [Display(Name = "Condición")]
        public string Condition { get; set; }

        [Required(ErrorMessage = "La probabilidad es obligatoria")]
        [Range(0, 100, ErrorMessage = "La probabilidad debe estar entre 0 y 100")]
        [Display(Name = "Probabilidad (%)")]
        public double Probability { get; set; }

        [Required(ErrorMessage = "Las recomendaciones son obligatorias")]
        [Display(Name = "Recomendaciones")]
        public List<string> Recommendations { get; set; } = new List<string>();

        // Propiedades calculadas
        [BsonIgnore]
        public bool IsHighProbability => Probability >= 70;

        [BsonIgnore]
        public bool IsModerateProbability => Probability >= 40 && Probability < 70;

        [BsonIgnore]
        public bool IsLowProbability => Probability < 40;

        [BsonIgnore]
        public int RecommendationsCount => Recommendations?.Count ?? 0;
    }

    /// <summary>
    /// Niveles de urgencia
    /// </summary>
    public enum UrgencyLevel
    {
        Low,      // Baja
        Medium,   // Media
        High,     // Alta
        Critical  // Crítica
    }
}

