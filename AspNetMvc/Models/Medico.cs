using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class Medico
    {
        [Key, ForeignKey("Usuario")]
        public int Id { get; set; }

        public string NumeroColegiado { get; set; }

        public int EspecialidadId { get; set; }

        public string Consultorio { get; set; }

        public TimeSpan HoraInicioTurno { get; set; }

        public TimeSpan HoraFinTurno { get; set; }

        public string DiasAtencion { get; set; } // "1,2,3,4,5" para lunes a viernes

        public int TiempoConsultaMinutos { get; set; }

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; }
        public virtual Especialidad Especialidad { get; set; }
        public virtual ICollection<Cita> Citas { get; set; }
        public virtual ICollection<HistorialMedico> HistorialesMedicos { get; set; }

        public Medico()
        {
            TiempoConsultaMinutos = 30;
            HoraInicioTurno = new TimeSpan(8, 0, 0);
            HoraFinTurno = new TimeSpan(17, 0, 0);
            DiasAtencion = "1,2,3,4,5";
            Citas = new List<Cita>();
            HistorialesMedicos = new List<HistorialMedico>();
        }
    }
}