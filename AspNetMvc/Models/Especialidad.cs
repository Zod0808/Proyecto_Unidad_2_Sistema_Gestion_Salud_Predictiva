using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class Especialidad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la especialidad es requerido")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activa { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Medico> Medicos { get; set; }
        public virtual ICollection<Cita> Citas { get; set; }

        public Especialidad()
        {
            Activa = true;
            Medicos = new List<Medico>();
            Citas = new List<Cita>();
        }
    }
}