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
}