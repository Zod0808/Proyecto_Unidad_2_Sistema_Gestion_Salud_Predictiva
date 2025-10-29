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
    /// Servicio para gestionar reportes de síntomas
    /// </summary>
    public class ReporteSintomasService
    {
        private readonly IMongoCollection<ReporteSintomas> _reportes;
        private readonly AIService _aiService;

        public ReporteSintomasService()
        {
            _reportes = MongoDBHelper.GetCollection<ReporteSintomas>("symptomreports");
            _aiService = new AIService();
        }

        /// <summary>
        /// Obtiene todos los reportes
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerTodos()
        {
            return await _reportes.Find(_ => true).SortByDescending(r => r.ReportedAt).ToListAsync();
        }

        /// <summary>
        /// Obtiene un reporte por ID
        /// </summary>
        public async Task<ReporteSintomas> ObtenerPorId(string id)
        {
            return await _reportes.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene reportes por usuario
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerPorUsuario(string userId)
        {
            return await _reportes.Find(r => r.UserId == userId)
                                 .SortByDescending(r => r.ReportedAt)
                                 .ToListAsync();
        }

        /// <summary>
        /// Crea un nuevo reporte con análisis de IA
        /// </summary>
        public async Task<ReporteSintomas> Crear(ReporteSintomas reporte)
        {
            reporte.CreatedAt = DateTime.Now;
            reporte.UpdatedAt = DateTime.Now;
            reporte.ReportedAt = DateTime.Now;
            reporte.Status = ReportStatus.Pending;

            // Intentar analizar con IA
            try
            {
                var aiRequest = new AIAnalysisRequest
                {
                    PatientName = reporte.PatientName,
                    Age = reporte.Age,
                    Gender = reporte.Gender,
                    Symptoms = reporte.Symptoms.Select(s => new SymptomsRequest
                    {
                        Name = s,
                        Severity = "moderate", // Default
                        Duration = "unknown",
                        Description = ""
                    }).ToList(),
                    AdditionalNotes = reporte.AdditionalNotes,
                    Location = reporte.Location != null ? new LocationRequest
                    {
                        Latitude = reporte.Location.Latitude,
                        Longitude = reporte.Location.Longitude,
                        Address = reporte.Location.Address
                    } : null
                };

                var aiResponse = await _aiService.AnalyzarSintomas(aiRequest);

                // Convertir respuesta de IA a modelo interno
                reporte.AIAnalysis = ConvertirRespuestaIA(aiResponse);
                reporte.Status = ReportStatus.InReview;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al analizar con IA: {ex.Message}");
                // Continuar sin análisis de IA
            }

            await _reportes.InsertOneAsync(reporte);
            return reporte;
        }

        /// <summary>
        /// Actualiza un reporte
        /// </summary>
        public async Task<bool> Actualizar(ReporteSintomas reporte)
        {
            reporte.UpdatedAt = DateTime.Now;
            var result = await _reportes.ReplaceOneAsync(r => r.Id == reporte.Id, reporte);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Elimina un reporte
        /// </summary>
        public async Task<bool> Eliminar(string id)
        {
            var result = await _reportes.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        /// <summary>
        /// Obtiene reportes urgentes
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerUrgentes()
        {
            var reportes = await ObtenerTodos();
            return reportes.Where(r => r.IsUrgent).ToList();
        }

        /// <summary>
        /// Obtiene reportes por estado
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerPorEstado(ReportStatus estado)
        {
            return await _reportes.Find(r => r.Status == estado)
                                 .SortByDescending(r => r.ReportedAt)
                                 .ToListAsync();
        }

        /// <summary>
        /// Obtiene reportes por rango de fechas
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _reportes.Find(r => r.ReportedAt >= fechaInicio && r.ReportedAt <= fechaFin)
                                 .SortByDescending(r => r.ReportedAt)
                                 .ToListAsync();
        }

        /// <summary>
        /// Obtiene estadísticas de reportes
        /// </summary>
        public async Task<Dictionary<string, object>> ObtenerEstadisticas()
        {
            var reportes = await ObtenerTodos();

            return new Dictionary<string, object>
            {
                { "Total", reportes.Count },
                { "Urgentes", reportes.Count(r => r.IsUrgent) },
                { "Pendientes", reportes.Count(r => r.Status == ReportStatus.Pending) },
                { "EnRevision", reportes.Count(r => r.Status == ReportStatus.InReview) },
                { "Revisados", reportes.Count(r => r.Status == ReportStatus.Reviewed) },
                { "Cerrados", reportes.Count(r => r.Status == ReportStatus.Closed) },
                { "ConAnalisisIA", reportes.Count(r => r.HasAIAnalysis) },
                { "ConUbicacion", reportes.Count(r => r.HasLocation) },
                { "PromedioSintomas", reportes.Any() ? reportes.Average(r => r.SymptomsCount) : 0 },
                { "SintomasMasComunes", ObtenerSintomasMasComunes(reportes, 10) }
            };
        }

        /// <summary>
        /// Obtiene los síntomas más comunes
        /// </summary>
        private List<dynamic> ObtenerSintomasMasComunes(List<ReporteSintomas> reportes, int cantidad)
        {
            return reportes.SelectMany(r => r.Symptoms)
                          .GroupBy(s => s)
                          .Select(g => new { Sintoma = g.Key, Cantidad = g.Count() })
                          .OrderByDescending(x => x.Cantidad)
                          .Take(cantidad)
                          .Cast<dynamic>()
                          .ToList();
        }

        /// <summary>
        /// Cambia el estado de un reporte
        /// </summary>
        public async Task<bool> CambiarEstado(string id, ReportStatus nuevoEstado)
        {
            var reporte = await ObtenerPorId(id);
            if (reporte == null) return false;

            reporte.Status = nuevoEstado;
            reporte.UpdatedAt = DateTime.Now;

            return await Actualizar(reporte);
        }

        /// <summary>
        /// Convierte la respuesta de IA al modelo interno
        /// </summary>
        private AnalisisIA ConvertirRespuestaIA(AIAnalysisResponse aiResponse)
        {
            var analisis = new AnalisisIA
            {
                MedicalHistoryId = aiResponse.AnalysisId,
                Confidence = aiResponse.Confidence,
                Urgency = ParseUrgency(aiResponse.Urgency),
                Timestamp = DateTime.TryParse(aiResponse.Timestamp, out DateTime timestamp) ? timestamp : DateTime.Now,
                PossibleDiagnoses = aiResponse.PossibleDiagnoses.Select(d => new DiagnosticoPosible
                {
                    Condition = d.Condition,
                    Probability = d.Probability,
                    Recommendations = d.Recommendations ?? new List<string>()
                }).ToList()
            };

            return analisis;
        }

        /// <summary>
        /// Convierte string de urgencia a enum
        /// </summary>
        private UrgencyLevel ParseUrgency(string urgency)
        {
            switch (urgency?.ToLower())
            {
                case "low":
                    return UrgencyLevel.Low;
                case "medium":
                    return UrgencyLevel.Medium;
                case "high":
                    return UrgencyLevel.High;
                case "critical":
                    return UrgencyLevel.Critical;
                default:
                    return UrgencyLevel.Low;
            }
        }
    }
}

