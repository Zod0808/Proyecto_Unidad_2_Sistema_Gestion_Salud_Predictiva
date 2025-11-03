using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers;
using MongoDB.Driver;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services
{
    public class ReporteSintomasService
    {
        private readonly IMongoCollection<ReporteSintomas> _reportesCollection;

        public ReporteSintomasService()
        {
            _reportesCollection = MongoDBHelper.GetCollection<ReporteSintomas>("symptomreports");
        }

        /// <summary>
        /// Crea un nuevo reporte de síntomas
        /// </summary>
        public async Task<ReporteSintomas> CrearReporte(ReporteSintomas reporte)
        {
            if (_reportesCollection == null)
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }

            // Calcular nivel de urgencia automáticamente
            reporte.NivelUrgencia = CalcularNivelUrgencia(reporte);
            
            // Generar recomendación basada en síntomas
            reporte.RecomendacionIA = GenerarRecomendacion(reporte);
            
            await _reportesCollection.InsertOneAsync(reporte);
            return reporte;
        }

        /// <summary>
        /// Obtiene todos los reportes de un paciente
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerPorPaciente(string pacienteId)
        {
            if (_reportesCollection == null)
            {
                return new List<ReporteSintomas>();
            }

            var filter = Builders<ReporteSintomas>.Filter.Eq(r => r.PacienteId, pacienteId);
            var reportes = await _reportesCollection
                .Find(filter)
                .SortByDescending(r => r.FechaReporte)
                .ToListAsync();
            
            return reportes;
        }

        /// <summary>
        /// Obtiene un reporte por ID
        /// </summary>
        public async Task<ReporteSintomas> ObtenerPorId(string reporteId)
        {
            if (_reportesCollection == null)
            {
                return null;
            }

            var filter = Builders<ReporteSintomas>.Filter.Eq(r => r.Id, reporteId);
            return await _reportesCollection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene todos los reportes (para médicos/admin)
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerTodos()
        {
            if (_reportesCollection == null)
            {
                return new List<ReporteSintomas>();
            }

            return await _reportesCollection
                .Find(_ => true)
                .SortByDescending(r => r.FechaReporte)
                .Limit(100)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene reportes urgentes (nivel 4-5)
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerUrgentes()
        {
            if (_reportesCollection == null)
            {
                return new List<ReporteSintomas>();
            }

            var filter = Builders<ReporteSintomas>.Filter.Gte(r => r.NivelUrgencia, 4);
            return await _reportesCollection
                .Find(filter)
                .SortByDescending(r => r.FechaReporte)
                .Limit(50)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene reportes por rango de fechas
        /// </summary>
        public async Task<List<ReporteSintomas>> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (_reportesCollection == null)
            {
                return new List<ReporteSintomas>();
            }

            var filter = Builders<ReporteSintomas>.Filter.And(
                Builders<ReporteSintomas>.Filter.Gte(r => r.FechaReporte, fechaInicio),
                Builders<ReporteSintomas>.Filter.Lte(r => r.FechaReporte, fechaFin)
            );

            return await _reportesCollection
                .Find(filter)
                .SortByDescending(r => r.FechaReporte)
                .ToListAsync();
        }

        /// <summary>
        /// Actualiza el estado de un reporte
        /// </summary>
        public async Task ActualizarEstado(string reporteId, string nuevoEstado)
        {
            if (_reportesCollection == null)
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }

            var filter = Builders<ReporteSintomas>.Filter.Eq(r => r.Id, reporteId);
            var update = Builders<ReporteSintomas>.Update
                .Set(r => r.Estado, nuevoEstado)
                .Set(r => r.Procesado, true);
            
            await _reportesCollection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Calcula el nivel de urgencia basado en los síntomas
        /// </summary>
        private int CalcularNivelUrgencia(ReporteSintomas reporte)
        {
            int puntaje = 0;

            foreach (var sintoma in reporte.Sintomas)
            {
                // Síntomas graves aumentan urgencia
                if (sintoma.Nombre.Contains("Dificultad para respirar") || 
                    sintoma.Nombre.Contains("Dolor de pecho"))
                {
                    puntaje += 3;
                }
                else if (sintoma.Nombre.Contains("Fiebre") || 
                         sintoma.Nombre.Contains("Falta de aire"))
                {
                    puntaje += 2;
                }
                else
                {
                    puntaje += 1;
                }

                // Gravedad aumenta urgencia
                if (sintoma.Gravedad == "Grave")
                {
                    puntaje += 2;
                }
                else if (sintoma.Gravedad == "Moderado")
                {
                    puntaje += 1;
                }

                // Duración prolongada aumenta urgencia
                if (sintoma.Duracion > 7)
                {
                    puntaje += 1;
                }
            }

            // Convertir puntaje a nivel 1-5
            if (puntaje >= 10) return 5; // Crítico
            if (puntaje >= 7) return 4;  // Urgente
            if (puntaje >= 4) return 3;  // Moderado-Alto
            if (puntaje >= 2) return 2;  // Moderado
            return 1; // Leve
        }

        /// <summary>
        /// Genera recomendación basada en síntomas
        /// </summary>
        private string GenerarRecomendacion(ReporteSintomas reporte)
        {
            var sintomasNombres = reporte.Sintomas.Select(s => s.Nombre.ToLower()).ToList();
            
            // Casos críticos
            if (sintomasNombres.Any(s => s.Contains("dificultad para respirar") && reporte.Sintomas.Any(si => si.Gravedad == "Grave")))
            {
                return "🚨 URGENTE: Dificultad respiratoria grave detectada. Acuda a URGENCIAS inmediatamente o llame al 911.";
            }

            if (sintomasNombres.Any(s => s.Contains("dolor de pecho")) && 
                reporte.Sintomas.Any(si => si.Gravedad == "Grave" || si.Gravedad == "Moderado"))
            {
                return "⚠️ IMPORTANTE: Dolor de pecho detectado. Acuda a urgencias para evaluación médica inmediata.";
            }

            // Fiebre alta
            if (sintomasNombres.Any(s => s.Contains("fiebre")))
            {
                var sintomaFiebre = reporte.Sintomas.FirstOrDefault(s => s.Nombre.ToLower().Contains("fiebre"));
                if (sintomaFiebre?.Gravedad == "Grave" || sintomaFiebre?.Duracion > 3)
                {
                    return "🌡️ Fiebre persistente o alta detectada. Programe una cita médica en las próximas 24 horas. Manténgase hidratado y monitoree su temperatura.";
                }
            }

            // Síntomas respiratorios múltiples
            if (reporte.Sintomas.Count >= 3 && sintomasNombres.Any(s => s.Contains("tos") || s.Contains("respirar")))
            {
                return "🩺 Múltiples síntomas respiratorios detectados. Programe una consulta médica en 24-48 horas. Mientras tanto, descanse, hidrátese y evite contacto cercano con otras personas.";
            }

            // Recomendación general
            return "📋 Síntomas registrados. Si los síntomas empeoran o no mejoran en 2-3 días, programe una consulta médica. Descanse, hidrátese bien y monitoree su estado.";
        }

        /// <summary>
        /// Obtiene estadísticas de reportes por ubicación
        /// </summary>
        public async Task<Dictionary<string, int>> ObtenerEstadisticasPorUbicacion()
        {
            if (_reportesCollection == null)
            {
                return new Dictionary<string, int>();
            }

            var reportes = await _reportesCollection.Find(_ => true).ToListAsync();
            
            return reportes
                .Where(r => !string.IsNullOrEmpty(r.Ubicacion))
                .GroupBy(r => r.Ubicacion)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Obtiene los síntomas más reportados
        /// </summary>
        public async Task<Dictionary<string, int>> ObtenerSintomasMasReportados()
        {
            if (_reportesCollection == null)
            {
                return new Dictionary<string, int>();
            }

            var reportes = await _reportesCollection.Find(_ => true).ToListAsync();
            
            var sintomas = reportes
                .SelectMany(r => r.Sintomas)
                .GroupBy(s => s.Nombre)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => g.Key, g => g.Count());
            
            return sintomas;
        }

        /// <summary>
        /// Obtiene estadísticas generales de reportes
        /// </summary>
        public async Task<Dictionary<string, object>> ObtenerEstadisticas()
        {
            if (_reportesCollection == null)
            {
                return new Dictionary<string, object>();
            }

            var reportes = await ObtenerTodos();
            
            return new Dictionary<string, object>
            {
                { "Total", reportes.Count },
                { "Urgentes", reportes.Count(r => r.NivelUrgencia >= 4) },
                { "PorFechas", reportes.GroupBy(r => r.FechaReporte.Date).ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count()) },
                { "SintomasMasReportados", await ObtenerSintomasMasReportados() },
                { "EstadisticasUbicacion", await ObtenerEstadisticasPorUbicacion() }
            };
        }
    }
}
