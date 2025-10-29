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
    /// Servicio para gestionar historiales médicos
    /// </summary>
    public class HistorialMedicoService
    {
        private readonly IMongoCollection<HistorialMedicoRespiCare> _historiales;

        public HistorialMedicoService()
        {
            _historiales = MongoDBHelper.GetCollection<HistorialMedicoRespiCare>("medicalhistories");
        }

        /// <summary>
        /// Obtiene todos los historiales
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerTodos()
        {
            return await _historiales.Find(_ => true).SortByDescending(h => h.Date).ToListAsync();
        }

        /// <summary>
        /// Obtiene un historial por ID
        /// </summary>
        public async Task<HistorialMedicoRespiCare> ObtenerPorId(string id)
        {
            return await _historiales.Find(h => h.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene historiales por paciente
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerPorPaciente(string patientId)
        {
            return await _historiales.Find(h => h.PatientId == patientId)
                                    .SortByDescending(h => h.Date)
                                    .ToListAsync();
        }

        /// <summary>
        /// Obtiene historiales por doctor
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerPorDoctor(string doctorId)
        {
            return await _historiales.Find(h => h.DoctorId == doctorId)
                                    .SortByDescending(h => h.Date)
                                    .ToListAsync();
        }

        /// <summary>
        /// Crea un nuevo historial
        /// </summary>
        public async Task<HistorialMedicoRespiCare> Crear(HistorialMedicoRespiCare historial)
        {
            historial.CreatedAt = DateTime.Now;
            historial.UpdatedAt = DateTime.Now;
            historial.Date = DateTime.Now;

            await _historiales.InsertOneAsync(historial);
            return historial;
        }

        /// <summary>
        /// Actualiza un historial
        /// </summary>
        public async Task<bool> Actualizar(HistorialMedicoRespiCare historial)
        {
            historial.UpdatedAt = DateTime.Now;
            var result = await _historiales.ReplaceOneAsync(h => h.Id == historial.Id, historial);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Elimina un historial
        /// </summary>
        public async Task<bool> Eliminar(string id)
        {
            var result = await _historiales.DeleteOneAsync(h => h.Id == id);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Obtiene historiales por rango de fechas
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _historiales.Find(h => h.Date >= fechaInicio && h.Date <= fechaFin)
                                    .SortByDescending(h => h.Date)
                                    .ToListAsync();
        }

        /// <summary>
        /// Obtiene historiales urgentes
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerUrgentes()
        {
            var historiales = await ObtenerTodos();
            return historiales.Where(h => h.IsUrgent).ToList();
        }

        /// <summary>
        /// Obtiene historiales pendientes de sincronización
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> ObtenerPendientesSincronizacion()
        {
            return await _historiales.Find(h => h.SyncStatus == SyncStatus.Pending || h.SyncStatus == SyncStatus.Error)
                                    .SortBy(h => h.CreatedAt)
                                    .ToListAsync();
        }

        /// <summary>
        /// Obtiene estadísticas de historiales
        /// </summary>
        public async Task<Dictionary<string, object>> ObtenerEstadisticas()
        {
            var historiales = await ObtenerTodos();
            
            return new Dictionary<string, object>
            {
                { "Total", historiales.Count },
                { "Urgentes", historiales.Count(h => h.IsUrgent) },
                { "Sincronizados", historiales.Count(h => h.IsSynced) },
                { "PendientesSincronizacion", historiales.Count(h => h.NeedsSync) },
                { "ConUbicacion", historiales.Count(h => h.HasLocation) },
                { "Offline", historiales.Count(h => h.IsOffline) },
                { "PromedioEdad", historiales.Any() ? historiales.Average(h => h.Age) : 0 },
                { "DiagnosticosMasComunes", ObtenerDiagnosticosMasComunes(historiales, 5) }
            };
        }

        /// <summary>
        /// Obtiene los diagnósticos más comunes
        /// </summary>
        private List<dynamic> ObtenerDiagnosticosMasComunes(List<HistorialMedicoRespiCare> historiales, int cantidad)
        {
            return historiales.GroupBy(h => h.Diagnosis)
                             .Select(g => new { Diagnostico = g.Key, Cantidad = g.Count() })
                             .OrderByDescending(x => x.Cantidad)
                             .Take(cantidad)
                             .Cast<dynamic>()
                             .ToList();
        }

        /// <summary>
        /// Busca historiales por texto
        /// </summary>
        public async Task<List<HistorialMedicoRespiCare>> Buscar(string texto)
        {
            var filter = Builders<HistorialMedicoRespiCare>.Filter.Or(
                Builders<HistorialMedicoRespiCare>.Filter.Regex(h => h.PatientName, new MongoDB.Bson.BsonRegularExpression(texto, "i")),
                Builders<HistorialMedicoRespiCare>.Filter.Regex(h => h.Diagnosis, new MongoDB.Bson.BsonRegularExpression(texto, "i")),
                Builders<HistorialMedicoRespiCare>.Filter.Regex(h => h.Description, new MongoDB.Bson.BsonRegularExpression(texto, "i"))
            );

            return await _historiales.Find(filter)
                                    .SortByDescending(h => h.Date)
                                    .ToListAsync();
        }

        /// <summary>
        /// Marca un historial como sincronizado
        /// </summary>
        public async Task<bool> MarcarComoSincronizado(string id)
        {
            var historial = await ObtenerPorId(id);
            if (historial == null) return false;

            historial.MarkAsSynced();
            return await Actualizar(historial);
        }

        /// <summary>
        /// Marca un historial con error
        /// </summary>
        public async Task<bool> MarcarConError(string id)
        {
            var historial = await ObtenerPorId(id);
            if (historial == null) return false;

            historial.MarkAsError();
            return await Actualizar(historial);
        }
    }
}

