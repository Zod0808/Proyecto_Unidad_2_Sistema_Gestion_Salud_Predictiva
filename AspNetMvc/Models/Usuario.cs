using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Usuario del Sistema RespiCare
    /// </summary>
    public class UsuarioRespiCare
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Rol")]
        [BsonRepresentation(BsonType.String)]
        public UserRole Role { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Último Login")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Fecha de Creación")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Actualización")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Propiedades calculadas
        [BsonIgnore]
        public bool IsAdmin => Role == UserRole.Admin;

        [BsonIgnore]
        public bool IsDoctor => Role == UserRole.Doctor;

        [BsonIgnore]
        public bool IsPatient => Role == UserRole.Patient;

        [BsonIgnore]
        public bool CanAccessMedicalData => IsDoctor || IsAdmin;

        [BsonIgnore]
        public bool CanManageUsers => IsAdmin;
    }

    /// <summary>
    /// Roles de usuario en el sistema
    /// </summary>
    public enum UserRole
    {
        Patient,
        Doctor,
        Admin
    }
}
