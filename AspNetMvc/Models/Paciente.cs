using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class Paciente
    {
        [Key, ForeignKey("Usuario")]
        public int Id { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Genero { get; set; }

        public string DireccionCompleta { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string ContactoEmergencia { get; set; }

        public string TelefonoEmergencia { get; set; }

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Cita> Citas { get; set; }
        public virtual ICollection<HistorialMedico> HistorialMedico { get; set; }
        public virtual ICollection<ChatIA> ChatsIA { get; set; }

        public int Edad 
        { 
            get 
            { 
                if (!FechaNacimiento.HasValue) return 0;
                var today = DateTime.Today;
                var age = today.Year - FechaNacimiento.Value.Year;
                if (FechaNacimiento.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public Paciente()
        {
            Citas = new List<Cita>();
            HistorialMedico = new List<HistorialMedico>();
            ChatsIA = new List<ChatIA>();
        }
    }
}