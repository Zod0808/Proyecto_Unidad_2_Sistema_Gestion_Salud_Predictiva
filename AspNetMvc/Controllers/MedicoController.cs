using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class MedicoController : BaseController
    {
        public ActionResult Dashboard()
        {
            var medicoId = ObtenerUsuarioId();
            
            var model = new MedicoDashboardViewModel
            {
                CitasHoy = ObtenerCitasHoy(medicoId),
                CitasPendientes = ObtenerCitasPendientes(medicoId),
                EstadisticasGenerales = ObtenerEstadisticasMedico(medicoId),
                ProximasCitas = ObtenerProximasCitas(medicoId, 7) // Próximos 7 días
            };

            return View(model);
        }

        public ActionResult MisCitas()
        {
            var medicoId = ObtenerUsuarioId();
            var fechaDesde = DateTime.Today;
            var fechaHasta = DateTime.Today.AddDays(30);
            
            var citas = ObtenerCitasMedico(medicoId, fechaDesde, fechaHasta);
            
            ViewBag.FechaDesde = fechaDesde.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta.ToString("yyyy-MM-dd");
            
            return View(citas);
        }

        [HttpPost]
        public ActionResult FiltrarCitas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var medicoId = ObtenerUsuarioId();
            var citas = ObtenerCitasMedico(medicoId, fechaDesde, fechaHasta);
            
            ViewBag.FechaDesde = fechaDesde.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta.ToString("yyyy-MM-dd");
            
            return View("MisCitas", citas);
        }

        public ActionResult DetalleCita(int id)
        {
            var medicoId = ObtenerUsuarioId();
            var cita = ObtenerCitaPorId(id, medicoId);
            
            if (cita == null)
            {
                return HttpNotFound();
            }

            // Obtener chat IA del paciente si existe
            var chatIA = ObtenerChatIAPaciente(cita.PacienteId);
            ViewBag.ChatIA = chatIA;
            
            return View(cita);
        }

        [HttpPost]
        public ActionResult ActualizarCita(int id, string diagnostico, string tratamiento, 
            string medicamentos, string observaciones, string signosVitales, string recomendaciones)
        {
            var medicoId = ObtenerUsuarioId();
            
            // Actualizar la cita y crear/actualizar historial médico
            ActualizarHistorialMedico(id, medicoId, diagnostico, tratamiento, 
                medicamentos, observaciones, signosVitales, recomendaciones);
            
            TempData["Mensaje"] = "Historial médico actualizado exitosamente";
            return RedirectToAction("DetalleCita", new { id = id });
        }

        public ActionResult MiHorario()
        {
            var medicoId = ObtenerUsuarioId();
            var medico = ObtenerMedicoPorId(medicoId);
            
            return View(medico);
        }

        [HttpPost]
        public ActionResult ActualizarHorario(TimeSpan horaInicio, TimeSpan horaFin, 
            string diasAtencion, int tiempoConsulta, string consultorio)
        {
            var medicoId = ObtenerUsuarioId();
            
            ActualizarHorarioMedico(medicoId, horaInicio, horaFin, diasAtencion, tiempoConsulta, consultorio);
            
            TempData["Mensaje"] = "Horario actualizado exitosamente";
            return RedirectToAction("MiHorario");
        }

        public ActionResult PacientesAtendidos()
        {
            var medicoId = ObtenerUsuarioId();
            var pacientes = ObtenerPacientesAtendidos(medicoId);
            
            return View(pacientes);
        }

        public ActionResult HistorialPaciente(int pacienteId)
        {
            var medicoId = ObtenerUsuarioId();
            
            // Verificar que el médico ha atendido al paciente
            if (!MedicoHaAtendidoPaciente(medicoId, pacienteId))
            {
                return new HttpUnauthorizedResult();
            }

            var historial = ObtenerHistorialCompletoPaciente(pacienteId);
            var paciente = ObtenerPacientePorId(pacienteId);
            
            ViewBag.Paciente = paciente;
            
            return View(historial);
        }

        public ActionResult ChatIAPaciente(int pacienteId, int? chatId = null)
        {
            var medicoId = ObtenerUsuarioId();
            
            // Verificar que el médico ha atendido al paciente
            if (!MedicoHaAtendidoPaciente(medicoId, pacienteId))
            {
                return new HttpUnauthorizedResult();
            }

            ChatIA chat;
            if (chatId.HasValue)
            {
                chat = ObtenerChatIAPorId(chatId.Value, pacienteId);
            }
            else
            {
                // Obtener el chat más reciente
                chat = ObtenerChatIAMasReciente(pacienteId);
            }

            var paciente = ObtenerPacientePorId(pacienteId);
            ViewBag.Paciente = paciente;
            ViewBag.TodosLosChats = ObtenerTodosLosChatsPaciente(pacienteId);
            
            return View(chat);
        }

        [HttpPost]
        public ActionResult ConfirmarCita(int citaId)
        {
            var medicoId = ObtenerUsuarioId();
            CambiarEstadoCita(citaId, medicoId, EstadoCita.Confirmada);
            
            TempData["Mensaje"] = "Cita confirmada exitosamente";
            return RedirectToAction("MisCitas");
        }

        [HttpPost]
        public ActionResult CancelarCita(int citaId, string motivo)
        {
            var medicoId = ObtenerUsuarioId();
            CancelarCitaMedico(citaId, medicoId, motivo);
            
            TempData["Mensaje"] = "Cita cancelada exitosamente";
            return RedirectToAction("MisCitas");
        }

        [HttpPost]
        public ActionResult IniciarConsulta(int citaId)
        {
            var medicoId = ObtenerUsuarioId();
            CambiarEstadoCita(citaId, medicoId, EstadoCita.EnCurso);
            
            return RedirectToAction("DetalleCita", new { id = citaId });
        }

        [HttpPost]
        public ActionResult FinalizarConsulta(int citaId)
        {
            var medicoId = ObtenerUsuarioId();
            CambiarEstadoCita(citaId, medicoId, EstadoCita.Completada);
            
            TempData["Mensaje"] = "Consulta finalizada exitosamente";
            return RedirectToAction("MisCitas");
        }

        #region Métodos privados

        private List<Cita> ObtenerCitasHoy(int medicoId)
        {
            var hoy = DateTime.Today;
            return ObtenerCitasMedico(medicoId, hoy, hoy.AddDays(1));
        }

        private List<Cita> ObtenerCitasPendientes(int medicoId)
        {
            // TODO: Implementar con base de datos
            return new List<Cita>
            {
                new Cita
                {
                    Id = 1,
                    FechaHora = DateTime.Now.AddHours(2),
                    Estado = EstadoCita.Confirmada,
                    MotivoConsulta = "Consulta general",
                    Paciente = new Paciente 
                    { 
                        Usuario = new Usuario { Nombre = "Juan", Apellido = "Pérez" } 
                    }
                }
            };
        }

        private EstadisticasMedico ObtenerEstadisticasMedico(int medicoId)
        {
            return new EstadisticasMedico
            {
                CitasHoy = 3,
                CitasSemana = 15,
                PacientesAtendidos = 45,
                PromedioConsultas = 4.2
            };
        }

        private List<Cita> ObtenerProximasCitas(int medicoId, int dias)
        {
            var fechaInicio = DateTime.Today.AddDays(1);
            var fechaFin = DateTime.Today.AddDays(dias);
            return ObtenerCitasMedico(medicoId, fechaInicio, fechaFin);
        }

        private List<Cita> ObtenerCitasMedico(int medicoId, DateTime fechaDesde, DateTime fechaHasta)
        {
            // TODO: Implementar con base de datos
            return new List<Cita>
            {
                new Cita
                {
                    Id = 1,
                    FechaHora = DateTime.Today.AddHours(10),
                    Estado = EstadoCita.Confirmada,
                    MotivoConsulta = "Consulta general",
                    Paciente = new Paciente 
                    { 
                        Usuario = new Usuario { Nombre = "Juan", Apellido = "Pérez", Telefono = "555-0001" },
                        FechaNacimiento = new DateTime(1985, 5, 15),
                        Genero = "Masculino"
                    }
                },
                new Cita
                {
                    Id = 2,
                    FechaHora = DateTime.Today.AddHours(14),
                    Estado = EstadoCita.Programada,
                    MotivoConsulta = "Control de seguimiento",
                    Paciente = new Paciente 
                    { 
                        Usuario = new Usuario { Nombre = "María", Apellido = "García", Telefono = "555-0002" },
                        FechaNacimiento = new DateTime(1990, 8, 22),
                        Genero = "Femenino"
                    }
                }
            };
        }

        private Cita ObtenerCitaPorId(int citaId, int medicoId)
        {
            // TODO: Implementar con base de datos
            // Verificar que la cita pertenece al médico
            return new Cita
            {
                Id = citaId,
                FechaHora = DateTime.Today.AddHours(10),
                Estado = EstadoCita.Confirmada,
                MotivoConsulta = "Consulta general",
                Observaciones = "Paciente refiere malestar general",
                Paciente = new Paciente 
                { 
                    Usuario = new Usuario { Nombre = "Juan", Apellido = "Pérez", Telefono = "555-0001" },
                    FechaNacimiento = new DateTime(1985, 5, 15),
                    Genero = "Masculino",
                    DireccionCompleta = "Calle 123 #45-67"
                },
                HistorialMedico = new HistorialMedico
                {
                    Diagnostico = "",
                    Sintomas = "",
                    Tratamiento = "",
                    Medicamentos = "",
                    SignosVitales = "",
                    Observaciones = ""
                }
            };
        }

        private ChatIA ObtenerChatIAPaciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new ChatIA
            {
                Id = 1,
                PacienteId = pacienteId,
                FechaInicio = DateTime.Now.AddHours(-3),
                ResumenTriaje = "Paciente consulta por malestar general, dolor de cabeza y fatiga.",
                SintomasReportados = "Dolor de cabeza, fatiga, malestar general",
                EvaluacionIA = "Síntomas sugieren posible cuadro viral o estrés",
                RecomendacionesIA = "Se recomienda consulta médica para evaluación",
                NivelUrgencia = 2,
                Completado = true,
                Mensajes = new List<MensajeChatIA>
                {
                    new MensajeChatIA
                    {
                        EsDelPaciente = true,
                        Mensaje = "Tengo dolor de cabeza desde hace 2 días",
                        Timestamp = DateTime.Now.AddHours(-3)
                    },
                    new MensajeChatIA
                    {
                        EsDelPaciente = false,
                        Mensaje = "¿El dolor es constante o intermitente? ¿En qué parte de la cabeza?",
                        Timestamp = DateTime.Now.AddHours(-3).AddMinutes(1)
                    }
                }
            };
        }

        private void ActualizarHistorialMedico(int citaId, int medicoId, string diagnostico, 
            string tratamiento, string medicamentos, string observaciones, string signosVitales, string recomendaciones)
        {
            // TODO: Implementar con base de datos
            // Crear o actualizar registro en HistorialMedico
            // Actualizar estado de la cita a completada
        }

        private Medico ObtenerMedicoPorId(int medicoId)
        {
            // TODO: Implementar con base de datos
            return new Medico
            {
                Id = medicoId,
                NumeroColegiado = "12345",
                Consultorio = "Consultorio 101",
                HoraInicioTurno = new TimeSpan(8, 0, 0),
                HoraFinTurno = new TimeSpan(17, 0, 0),
                DiasAtencion = "1,2,3,4,5", // Lunes a Viernes
                TiempoConsultaMinutos = 30,
                Usuario = new Usuario 
                { 
                    Nombre = "Dr. María", 
                    Apellido = "González",
                    Email = "medico@test.com",
                    Telefono = "555-0002"
                },
                Especialidad = new Especialidad { Nombre = "Medicina General" }
            };
        }

        private void ActualizarHorarioMedico(int medicoId, TimeSpan horaInicio, TimeSpan horaFin, 
            string diasAtencion, int tiempoConsulta, string consultorio)
        {
            // TODO: Implementar con base de datos
        }

        private List<Paciente> ObtenerPacientesAtendidos(int medicoId)
        {
            // TODO: Implementar con base de datos
            return new List<Paciente>
            {
                new Paciente
                {
                    Id = 1,
                    Usuario = new Usuario { Nombre = "Juan", Apellido = "Pérez" },
                    FechaNacimiento = new DateTime(1985, 5, 15)
                },
                new Paciente
                {
                    Id = 2,
                    Usuario = new Usuario { Nombre = "María", Apellido = "García" },
                    FechaNacimiento = new DateTime(1990, 8, 22)
                }
            };
        }

        private bool MedicoHaAtendidoPaciente(int medicoId, int pacienteId)
        {
            // TODO: Implementar verificación con base de datos
            return true; // Por ahora siempre true para pruebas
        }

        private List<HistorialMedico> ObtenerHistorialCompletoPaciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<HistorialMedico>
            {
                new HistorialMedico
                {
                    Id = 1,
                    Fecha = DateTime.Now.AddDays(-30),
                    Diagnostico = "Hipertensión arterial leve",
                    Tratamiento = "Cambios en el estilo de vida",
                    Medicamentos = "Enalapril 10mg",
                    Medico = new Medico 
                    { 
                        Usuario = new Usuario { Nombre = "Dr. Carlos", Apellido = "Ruiz" } 
                    }
                }
            };
        }

        private Paciente ObtenerPacientePorId(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new Paciente
            {
                Id = pacienteId,
                Usuario = new Usuario 
                { 
                    Nombre = "Juan", 
                    Apellido = "Pérez",
                    Email = "juan.perez@email.com",
                    Telefono = "555-0001"
                },
                FechaNacimiento = new DateTime(1985, 5, 15),
                Genero = "Masculino",
                DireccionCompleta = "Calle 123 #45-67",
                NumeroIdentificacion = "12345678",
                ContactoEmergencia = "Ana Pérez",
                TelefonoEmergencia = "555-0011"
            };
        }

        private ChatIA ObtenerChatIAPorId(int chatId, int pacienteId)
        {
            // TODO: Implementar con base de datos
            return ObtenerChatIAPaciente(pacienteId);
        }

        private ChatIA ObtenerChatIAMasReciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return ObtenerChatIAPaciente(pacienteId);
        }

        private List<ChatIA> ObtenerTodosLosChatsPaciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<ChatIA>
            {
                new ChatIA
                {
                    Id = 1,
                    FechaInicio = DateTime.Now.AddDays(-1),
                    ResumenTriaje = "Consulta por dolor de cabeza",
                    NivelUrgencia = 2,
                    Completado = true
                },
                new ChatIA
                {
                    Id = 2,
                    FechaInicio = DateTime.Now.AddDays(-7),
                    ResumenTriaje = "Consulta por síntomas gripales",
                    NivelUrgencia = 1,
                    Completado = true
                }
            };
        }

        private void CambiarEstadoCita(int citaId, int medicoId, EstadoCita nuevoEstado)
        {
            // TODO: Implementar con base de datos
            // Verificar que la cita pertenece al médico y cambiar estado
        }

        private void CancelarCitaMedico(int citaId, int medicoId, string motivo)
        {
            // TODO: Implementar con base de datos
            // Verificar que la cita pertenece al médico, cambiar estado y guardar motivo
        }

        #endregion
    }
}