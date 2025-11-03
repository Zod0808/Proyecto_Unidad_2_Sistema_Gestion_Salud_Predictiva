using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Diagnostics;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class ChatController : BaseController
    {
        private readonly ChatService _chatService;
        private readonly AIService _aiService;

        public ChatController()
        {
            _chatService = new ChatService();
            _aiService = new AIService();
        }

        /// <summary>
        /// Lista todas las conversaciones del usuario
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                var userId = ObtenerUsuarioId().ToString();
                List<ConversacionChat> conversaciones = new List<ConversacionChat>();
                
                try
                {
                    var conversacionesTask = _chatService.ObtenerPorUsuario(userId);
                    if (conversacionesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        conversaciones = conversacionesTask.Result;
                    }
                    else
                    {
                        Debug.WriteLine("Timeout al obtener conversaciones desde MongoDB");
                    }
                }
                catch (Exception mongoEx)
                {
                    Debug.WriteLine($"Error MongoDB en Chat Index: {mongoEx.Message}");
                    conversaciones = new List<ConversacionChat>();
                }

                return View(conversaciones);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar las conversaciones: " + ex.Message;
                return View(new List<ConversacionChat>());
            }
        }

        /// <summary>
        /// Abre una conversación específica
        /// </summary>
        public ActionResult Conversacion(string id)
        {
            try
            {
                ConversacionChat conversacion = null;
                
                try
                {
                    var conversacionTask = _chatService.ObtenerPorId(id);
                    if (conversacionTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        conversacion = conversacionTask.Result;
                    }
                    else
                    {
                        Debug.WriteLine("Timeout al obtener conversación desde MongoDB");
                    }
                }
                catch (Exception mongoEx)
                {
                    Debug.WriteLine($"Error MongoDB en Chat Conversacion: {mongoEx.Message}");
                }

                if (conversacion == null || conversacion.UserId != ObtenerUsuarioId().ToString())
                {
                    TempData["Error"] = "Conversación no encontrada";
                    return RedirectToAction("Index");
                }

                return View(conversacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar la conversación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Inicia una nueva conversación
        /// </summary>
        public ActionResult Nueva()
        {
            try
            {
                var userId = ObtenerUsuarioId().ToString();
                var userName = Session["UsuarioNombre"]?.ToString() ?? "Usuario";

                ConversacionChat nuevaConversacion = null;
                try
                {
                    var nuevaConversacionTask = _chatService.CrearDesdeTemplate(userId, userName);
                    if (nuevaConversacionTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        nuevaConversacion = nuevaConversacionTask.Result;
                    }
                    else
                    {
                        throw new TimeoutException("Timeout al crear conversación");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al crear conversación: {ex.Message}");
                    TempData["Error"] = "Error al crear la conversación. Por favor, intenta de nuevo.";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Conversacion", new { id = nuevaConversacion.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear la conversación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Envía un mensaje en la conversación (AJAX)
        /// </summary>
        [HttpPost]
        public JsonResult EnviarMensaje(string chatId, string mensaje)
        {
            // Asegurar UTF-8 en la respuesta
            Response.ContentType = "application/json; charset=utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            
            try
            {
                var userId = ObtenerUsuarioId().ToString();

                // Verificar que la conversación pertenece al usuario
                ConversacionChat conversacion = null;
                try
                {
                    var conversacionTask = _chatService.ObtenerPorId(chatId);
                    if (conversacionTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        conversacion = conversacionTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = "Error al obtener conversación: " + ex.Message });
                }

                if (conversacion == null || conversacion.UserId != userId)
                {
                    return Json(new { success = false, error = "Conversación no encontrada" });
                }

                // Agregar mensaje del usuario
                var mensajeUsuario = new MensajeChat
                {
                    Role = MessageRole.User,
                    Content = mensaje
                };

                try
                {
                    var agregarMensajeTask = _chatService.AgregarMensaje(chatId, mensajeUsuario);
                    if (!agregarMensajeTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        return Json(new { success = false, error = "Timeout al agregar mensaje" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = "Error al agregar mensaje: " + ex.Message });
                }

                // Generar respuesta de la IA
                string respuestaIA;
                try
                {
                    // Llamar al servicio de IA
                    var respuestaAITask = _aiService.GenerarRespuestaChat(mensaje, conversacion.Messages);
                    if (respuestaAITask.Wait(TimeSpan.FromSeconds(10)))
                    {
                        respuestaIA = respuestaAITask.Result;
                    }
                    else
                    {
                        throw new TimeoutException("Timeout en servicio de IA");
                    }
                }
                catch
                {
                    // Si falla el servicio de IA, usar respuesta genérica
                    respuestaIA = GenerarRespuestaGenerica(mensaje);
                }

                // Agregar respuesta de la IA
                var mensajeIA = new MensajeChat
                {
                    Role = MessageRole.Assistant,
                    Content = respuestaIA
                };

                try
                {
                    var agregarRespuestaTask = _chatService.AgregarMensaje(chatId, mensajeIA);
                    if (!agregarRespuestaTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        Debug.WriteLine("Timeout al agregar respuesta de IA");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al agregar respuesta de IA: {ex.Message}");
                }

                return Json(new { success = true, respuesta = respuestaIA });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Cierra una conversación
        /// </summary>
        [HttpPost]
        public ActionResult Cerrar(string id)
        {
            try
            {
                var userId = ObtenerUsuarioId().ToString();
                ConversacionChat conversacion = null;
                try
                {
                    var conversacionTask = _chatService.ObtenerPorId(id);
                    if (conversacionTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        conversacion = conversacionTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al obtener conversación para cerrar: {ex.Message}");
                }

                if (conversacion == null || conversacion.UserId != userId)
                {
                    TempData["Error"] = "Conversación no encontrada";
                    return RedirectToAction("Index");
                }

                try
                {
                    var cerrarTask = _chatService.Cerrar(id);
                    if (!cerrarTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        Debug.WriteLine("Timeout al cerrar conversación");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al cerrar conversación: {ex.Message}");
                }

                TempData["Success"] = "Conversación cerrada exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cerrar la conversación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Obtiene estadísticas de chat (AJAX)
        /// </summary>
        public JsonResult Estadisticas()
        {
            try
            {
                Dictionary<string, object> estadisticas = new Dictionary<string, object>();
                try
                {
                    var estadisticasTask = _chatService.ObtenerEstadisticas();
                    if (estadisticasTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        estadisticas = estadisticasTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al obtener estadísticas: {ex.Message}");
                }

                return Json(estadisticas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Genera una respuesta genérica cuando falla el servicio de IA
        /// </summary>
        private string GenerarRespuestaGenerica(string mensaje)
        {
            var respuestaGenerica = "Gracias por tu consulta. Necesito más información para poder ayudarte mejor. " +
                                   "¿Podrías describir con más detalle tus síntomas?";

            // Respuestas específicas basadas en palabras clave
            var mensajeLower = mensaje.ToLower();
            
            if (mensajeLower.Contains("dolor") || mensajeLower.Contains("duele"))
            {
                respuestaGenerica = "Entiendo que sientes dolor. ¿Podrías indicar dónde exactamente sientes el dolor " +
                                   "y desde cuándo comenzó?";
            }
            else if (mensajeLower.Contains("tos") || mensajeLower.Contains("tosiendo"))
            {
                respuestaGenerica = "Tienes tos. ¿Es una tos seca o con flemas? ¿Cuánto tiempo llevas con ella?";
            }
            else if (mensajeLower.Contains("dificultad") || mensajeLower.Contains("respirar"))
            {
                respuestaGenerica = "Si tienes dificultad para respirar, es importante que busques atención médica " +
                                   "lo antes posible. ¿La dificultad es leve o severa?";
            }
            else if (mensajeLower.Contains("fiebre") || mensajeLower.Contains("temperatura"))
            {
                respuestaGenerica = "Si tienes fiebre, es importante medir tu temperatura. ¿Cuál es tu temperatura " +
                                   "actual? ¿Desde cuándo tienes fiebre?";
            }

            return respuestaGenerica;
        }
    }
}

