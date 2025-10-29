using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class PacienteController : BaseController
    {
        public ActionResult Dashboard()
        {
            var pacienteId = ObtenerUsuarioId();
            var model = new PacienteDashboardViewModel
            {
                ProximasCitas = ObtenerProximasCitas(pacienteId),
                HistorialReciente = ObtenerHistorialReciente(pacienteId),
                ChatsIA = ObtenerChatsIARecientes(pacienteId),
                EstadisticasGenerales = new EstadisticasPaciente
                {
                    TotalCitas = 5,
                    CitasPendientes = 1,
                    UltimaConsulta = DateTime.Now.AddDays(-7)
                }
            };
            return View(model);
        }

        public ActionResult ChatIA()
        {
            var pacienteId = ObtenerUsuarioId();
            var chatsAnteriores = ObtenerChatsIA(pacienteId);
            return View(chatsAnteriores);
        }

        [HttpPost]
        public ActionResult IniciarNuevoChat()
        {
            var pacienteId = ObtenerUsuarioId();
            var nuevoChat = CrearNuevoChat(pacienteId);
            return RedirectToAction("Chat", new { id = nuevoChat.Id });
        }

        public ActionResult Chat(int id)
        {
            var pacienteId = ObtenerUsuarioId();
            var chat = ObtenerChatPorId(id, pacienteId);
            
            if (chat == null)
            {
                return HttpNotFound();
            }

            return View(chat);
        }

        [HttpPost]
        public JsonResult EnviarMensaje(int chatId, string mensaje)
        {
            var pacienteId = ObtenerUsuarioId();
            
            // Agregar mensaje del paciente
            AgregarMensaje(chatId, mensaje, true);
            
            // Generar respuesta de la IA (simulada)
            var respuestaIA = GenerarRespuestaIA(mensaje);
            AgregarMensaje(chatId, respuestaIA, false);

            return Json(new { success = true, respuesta = respuestaIA });
        }

        public ActionResult ReservarCita()
        {
            ViewBag.Especialidades = ObtenerEspecialidades();
            return View();
        }

        [HttpPost]
        public ActionResult ReservarCita(int especialidadId, DateTime fecha, string motivo)
        {
            var pacienteId = ObtenerUsuarioId();
            
            // Obtener médicos disponibles para esa especialidad
            var medicosDisponibles = ObtenerMedicosDisponibles(especialidadId, fecha);
            
            if (medicosDisponibles.Any())
            {
                var medico = medicosDisponibles.First(); // Seleccionar el primero disponible
                var cita = CrearCita(pacienteId, medico.Id, especialidadId, fecha, motivo);
                
                TempData["Mensaje"] = "Cita reservada exitosamente";
                return RedirectToAction("MisCitas");
            }
            else
            {
                TempData["Error"] = "No hay médicos disponibles para esa fecha y especialidad";
                ViewBag.Especialidades = ObtenerEspecialidades();
                return View();
            }
        }

        public ActionResult MisCitas()
        {
            var pacienteId = ObtenerUsuarioId();
            var citas = ObtenerCitasPaciente(pacienteId);
            return View(citas);
        }

        public ActionResult HistorialMedico()
        {
            var pacienteId = ObtenerUsuarioId();
            var historial = ObtenerHistorialCompleto(pacienteId);
            return View(historial);
        }

        [HttpPost]
        public ActionResult CancelarCita(int citaId, string motivo)
        {
            var pacienteId = ObtenerUsuarioId();
            CancelarCitaPaciente(citaId, pacienteId, motivo);
            TempData["Mensaje"] = "Cita cancelada exitosamente";
            return RedirectToAction("MisCitas");
        }

        #region Métodos privados

        private List<Cita> ObtenerProximasCitas(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<Cita>
            {
                new Cita
                {
                    Id = 1,
                    FechaHora = DateTime.Now.AddDays(3),
                    Estado = EstadoCita.Confirmada,
                    MotivoConsulta = "Consulta general",
                    Especialidad = new Especialidad { Nombre = "Medicina General" }
                }
            };
        }

        private List<HistorialMedico> ObtenerHistorialReciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<HistorialMedico>
            {
                new HistorialMedico
                {
                    Id = 1,
                    Fecha = DateTime.Now.AddDays(-7),
                    Diagnostico = "Chequeo preventivo",
                    Medico = new Medico 
                    { 
                        Usuario = new Usuario { Nombre = "Dr. María", Apellido = "González" } 
                    }
                }
            };
        }

        private List<ChatIA> ObtenerChatsIARecientes(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<ChatIA>
            {
                new ChatIA
                {
                    Id = 1,
                    FechaInicio = DateTime.Now.AddHours(-2),
                    ResumenTriaje = "Consulta por dolor de cabeza",
                    NivelUrgencia = 2,
                    Completado = true
                }
            };
        }

        private EstadisticasPaciente ObtenerEstadisticasGenerales(int pacienteId)
        {
            return new EstadisticasPaciente
            {
                TotalCitas = 5,
                CitasPendientes = 1,
                UltimaConsulta = DateTime.Now.AddDays(-7)
            };
        }

        private List<ChatIA> ObtenerChatsIA(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<ChatIA>
            {
                new ChatIA
                {
                    Id = 1,
                    FechaInicio = DateTime.Now.AddDays(-1),
                    ResumenTriaje = "Consulta por síntomas gripales",
                    NivelUrgencia = 2,
                    Completado = true
                },
                new ChatIA
                {
                    Id = 2,
                    FechaInicio = DateTime.Now.AddHours(-3),
                    ResumenTriaje = "Dolor abdominal",
                    NivelUrgencia = 3,
                    Completado = false
                }
            };
        }

        private ChatIA CrearNuevoChat(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new ChatIA
            {
                Id = new Random().Next(100, 999),
                PacienteId = pacienteId,
                FechaInicio = DateTime.Now,
                Completado = false
            };
        }

        private ChatIA ObtenerChatPorId(int chatId, int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new ChatIA
            {
                Id = chatId,
                PacienteId = pacienteId,
                FechaInicio = DateTime.Now.AddMinutes(-30),
                Mensajes = new List<MensajeChatIA>
                {
                    new MensajeChatIA
                    {
                        Id = 1,
                        EsDelPaciente = false,
                        Mensaje = "Hola, soy tu asistente médico virtual. ¿En qué puedo ayudarte hoy?",
                        Timestamp = DateTime.Now.AddMinutes(-30)
                    }
                }
            };
        }

        private void AgregarMensaje(int chatId, string mensaje, bool esDelPaciente)
        {
            // TODO: Implementar con base de datos
            // Por ahora solo simulamos el guardado
        }

        private string GenerarRespuestaIA(string mensaje)
        {
            // TODO: Implementar integración con IA real
            // Por ahora respuestas simuladas
            var respuestas = new[]
            {
                "Entiendo. ¿Puedes contarme más detalles sobre tus síntomas?",
                "¿Desde cuándo experimentas estos síntomas?",
                "¿Has tomado algún medicamento para estos síntomas?",
                "¿Hay algo específico que empeore o mejore los síntomas?",
                "Basándome en la información proporcionada, te recomiendo agendar una cita con un médico."
            };
            
            return respuestas[new Random().Next(respuestas.Length)];
        }

        private List<Especialidad> ObtenerEspecialidades()
        {
            return new List<Especialidad>
            {
                new Especialidad { Id = 1, Nombre = "Medicina General" },
                new Especialidad { Id = 2, Nombre = "Cardiología" },
                new Especialidad { Id = 3, Nombre = "Dermatología" },
                new Especialidad { Id = 4, Nombre = "Pediatría" },
                new Especialidad { Id = 5, Nombre = "Ginecología" },
                new Especialidad { Id = 6, Nombre = "Traumatología" }
            };
        }

        private List<Medico> ObtenerMedicosDisponibles(int especialidadId, DateTime fecha)
        {
            // TODO: Implementar verificación real de disponibilidad
            return new List<Medico>
            {
                new Medico
                {
                    Id = 1,
                    EspecialidadId = especialidadId,
                    Usuario = new Usuario { Nombre = "Dr. Juan", Apellido = "Martínez" }
                }
            };
        }

        private Cita CrearCita(int pacienteId, int medicoId, int especialidadId, DateTime fecha, string motivo)
        {
            // TODO: Implementar con base de datos
            return new Cita
            {
                Id = new Random().Next(1000, 9999),
                PacienteId = pacienteId,
                MedicoId = medicoId,
                EspecialidadId = especialidadId,
                FechaHora = fecha,
                MotivoConsulta = motivo,
                Estado = EstadoCita.Programada
            };
        }

        private List<Cita> ObtenerCitasPaciente(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<Cita>
            {
                new Cita
                {
                    Id = 1,
                    FechaHora = DateTime.Now.AddDays(3),
                    Estado = EstadoCita.Confirmada,
                    MotivoConsulta = "Consulta general",
                    Medico = new Medico 
                    { 
                        Usuario = new Usuario { Nombre = "Dr. María", Apellido = "González" } 
                    },
                    Especialidad = new Especialidad { Nombre = "Medicina General" }
                }
            };
        }

        private List<HistorialMedico> ObtenerHistorialCompleto(int pacienteId)
        {
            // TODO: Implementar con base de datos
            return new List<HistorialMedico>
            {
                new HistorialMedico
                {
                    Id = 1,
                    Fecha = DateTime.Now.AddDays(-7),
                    Diagnostico = "Chequeo preventivo - Sin hallazgos patológicos",
                    Sintomas = "Ninguno reportado",
                    Tratamiento = "Mantener hábitos saludables",
                    Medico = new Medico 
                    { 
                        Usuario = new Usuario { Nombre = "Dr. María", Apellido = "González" } 
                    }
                }
            };
        }

        private void CancelarCitaPaciente(int citaId, int pacienteId, string motivo)
        {
            // TODO: Implementar con base de datos
            // Verificar que la cita pertenece al paciente y cancelarla
        }

        #endregion
    }
}