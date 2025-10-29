using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Usuario para el sistema antiguo (Heladería/Médicos)
    /// Coexiste con UsuarioRespiCare del sistema nuevo
    /// </summary>
    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            // Inicialización de colecciones si es necesario
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        public TipoUsuario TipoUsuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        [StringLength(10)]
        [Column("cod_empleado")]
        public string cod_empleado { get; set; }

        [StringLength(20)]
        public string nombre { get; set; }

        [StringLength(255)]
        public string clave { get; set; }

        [StringLength(10)]
        public string nivel { get; set; }

        [StringLength(1)]
        public string estado { get; set; }

        // Propiedades de navegación
        public virtual Empleado Empleado { get; set; }
        public virtual Paciente Paciente { get; set; }
        public virtual Medico Medico { get; set; }

        // Propiedad calculada
        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                if (!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Apellido))
                {
                    return $"{Nombre} {Apellido}";
                }
                else if (!string.IsNullOrEmpty(nombre))
                {
                    return nombre;
                }
                return Email;
            }
        }
    }

    public enum TipoUsuario
    {
        Paciente = 1,
        Medico = 2,
        Administrador = 3
    }
}

