using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }

        public bool RecordarMe { get; set; }
    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirme la contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de usuario")]
        public TipoUsuario TipoUsuario { get; set; }

        // Campos adicionales para Paciente
        public System.DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string DireccionCompleta { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }

        // Campos adicionales para Médico
        public string NumeroColegiado { get; set; }
        public int? EspecialidadId { get; set; }
        public string Consultorio { get; set; }
    }

    /// <summary>
    /// ViewModel para el perfil del paciente
    /// </summary>
    public class PerfilPacienteViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre completo es requerido")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        
        public int Edad => DateTime.Now.Year - FechaNacimiento.Year;
        
        [Display(Name = "Género")]
        public string Genero { get; set; }
        
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        
        [Display(Name = "Grupo Sanguíneo")]
        public string GrupoSanguineo { get; set; }
        
        [Display(Name = "Alergias Conocidas")]
        [DataType(DataType.MultilineText)]
        public string Alergias { get; set; }
        
        [Display(Name = "Medicamentos Actuales")]
        [DataType(DataType.MultilineText)]
        public string MedicamentosActuales { get; set; }
        
        [Display(Name = "Condiciones Crónicas")]
        [DataType(DataType.MultilineText)]
        public string CondicionesCronicas { get; set; }
        
        [Display(Name = "Contacto de Emergencia")]
        public string ContactoEmergencia { get; set; }
        
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [Display(Name = "Teléfono de Emergencia")]
        public string TelefonoEmergencia { get; set; }
        
        public DateTime FechaRegistro { get; set; }
    }
}