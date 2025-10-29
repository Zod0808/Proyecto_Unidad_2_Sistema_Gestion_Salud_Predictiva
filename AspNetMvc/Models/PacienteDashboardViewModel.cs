using System;
using System.Collections.Generic;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public class PacienteDashboardViewModel
    {
        public EstadisticasPaciente EstadisticasGenerales { get; set; }
        public List<Cita> ProximasCitas { get; set; }
        public List<HistorialMedico> HistorialReciente { get; set; }
        public List<ChatIA> ChatsIA { get; set; }
    }

    public class EstadisticasPaciente
    {
        public int TotalCitas { get; set; }
        public int CitasPendientes { get; set; }
        public DateTime UltimaConsulta { get; set; }
    }

    public class MedicoDashboardViewModel
    {
        public List<Cita> CitasHoy { get; set; }
        public List<Cita> CitasPendientes { get; set; }
        public List<Cita> ProximasCitas { get; set; }
        public EstadisticasMedico EstadisticasGenerales { get; set; }

        public MedicoDashboardViewModel()
        {
            CitasHoy = new List<Cita>();
            CitasPendientes = new List<Cita>();
            ProximasCitas = new List<Cita>();
            EstadisticasGenerales = new EstadisticasMedico();
        }
    }

    public class EstadisticasMedico
    {
        public int CitasHoy { get; set; }
        public int CitasSemana { get; set; }
        public int PacientesAtendidos { get; set; }
        public double PromedioConsultas { get; set; }
    }
}
