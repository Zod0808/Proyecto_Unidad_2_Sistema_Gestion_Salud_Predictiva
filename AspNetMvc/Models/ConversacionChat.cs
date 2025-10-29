using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Conversación de Chat
    /// </summary>
    public class ConversacionChat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        [Display(Name = "ID del Usuario")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Usuario")]
        public string UserName { get; set; }

        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Debe tener al menos un mensaje")]
        [Display(Name = "Mensajes")]
        public List<MensajeChat> Messages { get; set; } = new List<MensajeChat>();

        [Display(Name = "Activa")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Análisis IA Generado")]
        public string AIAnalysisId { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartedAt { get; set; } = DateTime.Now;

        [Display(Name = "Última Actividad")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastActivity { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Creación")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Actualización")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Propiedades calculadas
        [BsonIgnore]
        public int MessagesCount => Messages?.Count ?? 0;

        [BsonIgnore]
        public bool HasAIAnalysis => !string.IsNullOrEmpty(AIAnalysisId);

        [BsonIgnore]
        public TimeSpan Duration => DateTime.Now - StartedAt;

        [BsonIgnore]
        public string DurationText
        {
            get
            {
                var duration = Duration;
                if (duration.TotalMinutes < 1)
                    return "Menos de un minuto";
                if (duration.TotalHours < 1)
                    return $"{(int)duration.TotalMinutes} minutos";
                if (duration.TotalDays < 1)
                    return $"{(int)duration.TotalHours} horas";
                return $"{(int)duration.TotalDays} días";
            }
        }

        // Métodos de negocio
        public void AddMessage(MensajeChat message)
        {
            if (Messages == null)
                Messages = new List<MensajeChat>();

            Messages.Add(message);
            LastActivity = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void Close()
        {
            IsActive = false;
            UpdatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Modelo de Mensaje de Chat
    /// </summary>
    public class MensajeChat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        [BsonRepresentation(BsonType.String)]
        public MessageRole Role { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Display(Name = "Marca de Tiempo")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Display(Name = "Metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        // Propiedades calculadas
        [BsonIgnore]
        public bool IsUser => Role == MessageRole.User;

        [BsonIgnore]
        public bool IsAssistant => Role == MessageRole.Assistant;

        [BsonIgnore]
        public bool IsSystem => Role == MessageRole.System;

        [BsonIgnore]
        public string RoleText
        {
            get
            {
                switch (Role)
                {
                    case MessageRole.User:
                        return "Usuario";
                    case MessageRole.Assistant:
                        return "Asistente";
                    case MessageRole.System:
                        return "Sistema";
                    default:
                        return "Desconocido";
                }
            }
        }
    }

    /// <summary>
    /// Roles de mensaje
    /// </summary>
    public enum MessageRole
    {
        User,      // Usuario
        Assistant, // Asistente IA
        System     // Sistema
    }
}

