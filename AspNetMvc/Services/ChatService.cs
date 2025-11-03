using MongoDB.Driver;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services
{
    /// <summary>
    /// Servicio para gestionar conversaciones de chat con IA
    /// </summary>
    public class ChatService
    {
        private readonly IMongoCollection<ConversacionChat> _chats;

        public ChatService()
        {
            _chats = MongoDBHelper.GetCollection<ConversacionChat>("chatconversations");
        }

        /// <summary>
        /// Obtiene todas las conversaciones
        /// </summary>
        public async Task<List<ConversacionChat>> ObtenerTodos()
        {
            return await _chats.Find(_ => true).SortByDescending(c => c.LastActivity).ToListAsync();
        }

        /// <summary>
        /// Obtiene una conversación por ID
        /// </summary>
        public async Task<ConversacionChat> ObtenerPorId(string id)
        {
            return await _chats.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene conversaciones por usuario
        /// </summary>
        public async Task<List<ConversacionChat>> ObtenerPorUsuario(string userId)
        {
            return await _chats.Find(c => c.UserId == userId)
                              .SortByDescending(c => c.LastActivity)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene conversaciones activas del usuario
        /// </summary>
        public async Task<List<ConversacionChat>> ObtenerActivasPorUsuario(string userId)
        {
            return await _chats.Find(c => c.UserId == userId && c.IsActive)
                              .SortByDescending(c => c.LastActivity)
                              .ToListAsync();
        }

        /// <summary>
        /// Crea una nueva conversación
        /// </summary>
        public async Task<ConversacionChat> Crear(ConversacionChat chat)
        {
            chat.CreatedAt = DateTime.Now;
            chat.UpdatedAt = DateTime.Now;
            chat.StartedAt = DateTime.Now;
            chat.LastActivity = DateTime.Now;

            await _chats.InsertOneAsync(chat);
            return chat;
        }

        /// <summary>
        /// Actualiza una conversación
        /// </summary>
        public async Task<bool> Actualizar(ConversacionChat chat)
        {
            chat.UpdatedAt = DateTime.Now;
            var result = await _chats.ReplaceOneAsync(c => c.Id == chat.Id, chat);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Elimina una conversación
        /// </summary>
        public async Task<bool> Eliminar(string id)
        {
            var result = await _chats.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Agrega un mensaje a una conversación
        /// </summary>
        public async Task<bool> AgregarMensaje(string chatId, MensajeChat mensaje)
        {
            mensaje.Timestamp = DateTime.Now;
            var conversacion = await ObtenerPorId(chatId);
            
            if (conversacion == null)
                return false;

            conversacion.AddMessage(mensaje);
            return await Actualizar(conversacion);
        }

        /// <summary>
        /// Cierra una conversación
        /// </summary>
        public async Task<bool> Cerrar(string id)
        {
            var conversacion = await ObtenerPorId(id);
            if (conversacion == null)
                return false;

            conversacion.Close();
            return await Actualizar(conversacion);
        }

        /// <summary>
        /// Busca conversaciones por texto
        /// </summary>
        public async Task<List<ConversacionChat>> Buscar(string texto)
        {
            var filter = Builders<ConversacionChat>.Filter.Or(
                Builders<ConversacionChat>.Filter.Regex(c => c.Title, new MongoDB.Bson.BsonRegularExpression(texto, "i")),
                Builders<ConversacionChat>.Filter.Regex(c => c.UserName, new MongoDB.Bson.BsonRegularExpression(texto, "i"))
            );

            return await _chats.Find(filter)
                              .SortByDescending(c => c.LastActivity)
                              .ToListAsync();
        }

        /// <summary>
        /// Obtiene estadísticas de conversaciones
        /// </summary>
        public async Task<Dictionary<string, object>> ObtenerEstadisticas()
        {
            var conversaciones = await ObtenerTodos();

            return new Dictionary<string, object>
            {
                { "Total", conversaciones.Count },
                { "Activas", conversaciones.Count(c => c.IsActive) },
                { "Cerradas", conversaciones.Count(c => !c.IsActive) },
                { "ConAnálisisIA", conversaciones.Count(c => c.HasAIAnalysis) },
                { "PromedioMensajes", conversaciones.Any() ? conversaciones.Average(c => c.MessagesCount) : 0 },
                { "TiempoPromedio", conversaciones.Any() ? conversaciones.Average(c => c.Duration.TotalMinutes) : 0 }
            };
        }

        /// <summary>
        /// Crea una nueva conversación desde un template
        /// </summary>
        public async Task<ConversacionChat> CrearDesdeTemplate(string userId, string userName, string title = null)
        {
            var nuevaConversacion = new ConversacionChat
            {
                UserId = userId,
                UserName = userName,
                Title = title ?? $"Conversación {DateTime.Now:dd/MM/yyyy HH:mm}",
                IsActive = true
            };

            // Agregar mensaje de bienvenida del sistema
            var mensajeBienvenida = new MensajeChat
            {
                Role = MessageRole.System,
                Content = "¡Hola! Soy tu asistente médico virtual. Estoy aquí para ayudarte a evaluar tus síntomas respiratorios. ¿Podrías describir qué síntomas estás experimentando?"
            };

            nuevaConversacion.Messages.Add(mensajeBienvenida);

            return await Crear(nuevaConversacion);
        }
    }
}

