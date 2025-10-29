using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Historial Médico del Sistema RespiCare
    /// </summary>
    public class HistorialMedicoRespiCare
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El ID del paciente es obligatorio")]
        [Display(Name = "ID del Paciente")]
        public string PatientId { get; set; }

        [Required(ErrorMessage = "El ID del doctor es obligatorio")]
        [Display(Name = "ID del Doctor")]
        public string DoctorId { get; set; }

        [Required(ErrorMessage = "El nombre del paciente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Paciente")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Range(0, 150, ErrorMessage = "La edad debe estar entre 0 y 150 años")]
        [Display(Name = "Edad")]
        public int Age { get; set; }

        [Required(ErrorMessage = "El diagnóstico es obligatorio")]
        [StringLength(200, ErrorMessage = "El diagnóstico no puede exceder 200 caracteres")]
        [Display(Name = "Diagnóstico")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Debe tener al menos un síntoma")]
        [Display(Name = "Síntomas")]
        public List<Sintoma> Symptoms { get; set; } = new List<Sintoma>();

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Ubicación")]
        public Ubicacion Location { get; set; }

        [Display(Name = "Imágenes")]
        public List<string> Images { get; set; } = new List<string>();

        [Display(Name = "Notas de Audio")]
        public string AudioNotes { get; set; }

        [Display(Name = "Es Offline")]
        public bool IsOffline { get; set; } = false;

        [Display(Name = "Estado de Sincronización")]
        [BsonRepresentation(BsonType.String)]
        public SyncStatus SyncStatus { get; set; } = SyncStatus.Pending;

        [Display(Name = "Fecha de Creación")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Actualización")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Propiedades calculadas
        [BsonIgnore]
        public bool IsUrgent => Symptoms != null && Symptoms.Any(s => s.IsSevere);

        [BsonIgnore]
        public bool HasLocation => Location != null && Location.HasCoordinates;

        [BsonIgnore]
        public bool IsSynced => SyncStatus == SyncStatus.Synced;

        [BsonIgnore]
        public bool NeedsSync => SyncStatus == SyncStatus.Pending || SyncStatus == SyncStatus.Error;

        [BsonIgnore]
        public bool CanBeEdited => SyncStatus != SyncStatus.Synced || IsOffline;

        [BsonIgnore]
        public string SyncStatusText
        {
            get
            {
                switch (SyncStatus)
                {
                    case SyncStatus.Pending:
                        return "Pendiente";
                    case SyncStatus.Synced:
                        return "Sincronizado";
                    case SyncStatus.Error:
                        return "Error";
                    default:
                        return "Desconocido";
                }
            }
        }

        [BsonIgnore]
        public int SymptomsCount => Symptoms?.Count ?? 0;

        [BsonIgnore]
        public int ImagesCount => Images?.Count ?? 0;

        // Métodos de negocio
        public void AddSymptom(Sintoma symptom)
        {
            if (Symptoms == null)
                Symptoms = new List<Sintoma>();

            if (Symptoms.Count >= 20)
                throw new InvalidOperationException("No se pueden agregar más de 20 síntomas");

            Symptoms.Add(symptom);
            UpdatedAt = DateTime.Now;
        }

        public void MarkAsSynced()
        {
            SyncStatus = SyncStatus.Synced;
            UpdatedAt = DateTime.Now;
        }

        public void MarkAsError()
        {
            SyncStatus = SyncStatus.Error;
            UpdatedAt = DateTime.Now;
        }

        public void AddImage(string imageUrl)
        {
            if (Images == null)
                Images = new List<string>();

            Images.Add(imageUrl);
            UpdatedAt = DateTime.Now;
        }

        public bool IsValid()
        {
            return Age >= 0 && Age <= 150 &&
                   !string.IsNullOrWhiteSpace(Diagnosis) && Diagnosis.Length <= 200 &&
                   !string.IsNullOrWhiteSpace(PatientName) && PatientName.Length <= 100 &&
                   (string.IsNullOrWhiteSpace(Description) || Description.Length <= 1000) &&
                   Symptoms != null && Symptoms.Count > 0 && Symptoms.Count <= 20;
        }
    }

    /// <summary>
    /// Estados de sincronización
    /// </summary>
    public enum SyncStatus
    {
        Pending,  // Pendiente
        Synced,   // Sincronizado
        Error     // Error
    }
}

