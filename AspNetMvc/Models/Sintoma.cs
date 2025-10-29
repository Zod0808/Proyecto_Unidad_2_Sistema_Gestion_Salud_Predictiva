using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Síntoma
    /// </summary>
    public class Sintoma
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre del síntoma es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Síntoma")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La severidad es obligatoria")]
        [Display(Name = "Severidad")]
        [BsonRepresentation(BsonType.String)]
        public SeverityLevel Severity { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Display(Name = "Duración")]
        public string Duration { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        // Propiedades calculadas
        [BsonIgnore]
        public string SeverityText
        {
            get
            {
                switch (Severity)
                {
                    case SeverityLevel.Mild:
                        return "Leve";
                    case SeverityLevel.Moderate:
                        return "Moderado";
                    case SeverityLevel.Severe:
                        return "Severo";
                    default:
                        return "Desconocido";
                }
            }
        }

        [BsonIgnore]
        public bool IsSevere => Severity == SeverityLevel.Severe;

        [BsonIgnore]
        public bool RequiresImmediateAttention => Severity == SeverityLevel.Severe;
    }

    /// <summary>
    /// Niveles de severidad de un síntoma
    /// </summary>
    public enum SeverityLevel
    {
        Mild,      // Leve
        Moderate,  // Moderado
        Severe     // Severo
    }
}

