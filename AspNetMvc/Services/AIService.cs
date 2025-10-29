using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services
{
    /// <summary>
    /// Servicio para integración con IA (Python FastAPI)
    /// </summary>
    public class AIService
    {
        /// <summary>
        /// Analiza síntomas y retorna diagnósticos posibles
        /// </summary>
        public async Task<AIAnalysisResponse> AnalyzarSintomas(AIAnalysisRequest request)
        {
            try
            {
                var response = await HttpClientHelper.PostAsync<AIAnalysisRequest, AIAnalysisResponse>(
                    "/api/v1/analyze",
                    request
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al analizar síntomas con IA: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene predicciones ML para enfermedades
        /// </summary>
        public async Task<MLPredictionResponse> ObtenerPrediccionML(MLPredictionRequest request)
        {
            try
            {
                var response = await HttpClientHelper.PostAsync<MLPredictionRequest, MLPredictionResponse>(
                    "/api/v1/ml/predict",
                    request
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener predicción ML: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Procesa consulta de chatbot médico
        /// </summary>
        public async Task<ChatBotResponse> ProcesarConsultaChatBot(ChatBotRequest request)
        {
            try
            {
                var response = await HttpClientHelper.PostAsync<ChatBotRequest, ChatBotResponse>(
                    "/api/v1/chat/process",
                    request
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar consulta de chatbot: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene información de salud del servicio de IA
        /// </summary>
        public async Task<AIHealthResponse> ObtenerEstadoServicio()
        {
            try
            {
                var response = await HttpClientHelper.GetAsync<AIHealthResponse>("/api/v1/health/detailed");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estado del servicio de IA: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si el servicio de IA está disponible
        /// </summary>
        public async Task<bool> EstaDisponible()
        {
            return await HttpClientHelper.IsAIServiceAvailable();
        }
    }

    #region Request/Response Models para AI

    /// <summary>
    /// Request para análisis de síntomas
    /// </summary>
    public class AIAnalysisRequest
    {
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public List<SymptomsRequest> Symptoms { get; set; }
        public string AdditionalNotes { get; set; }
        public LocationRequest Location { get; set; }
    }

    public class SymptomsRequest
    {
        public string Name { get; set; }
        public string Severity { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }

    public class LocationRequest
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
    }

    /// <summary>
    /// Response de análisis de síntomas
    /// </summary>
    public class AIAnalysisResponse
    {
        public string AnalysisId { get; set; }
        public List<DiagnosisResponse> PossibleDiagnoses { get; set; }
        public string Urgency { get; set; }
        public double Confidence { get; set; }
        public string Timestamp { get; set; }
        public List<string> GeneralRecommendations { get; set; }
    }

    public class DiagnosisResponse
    {
        public string Condition { get; set; }
        public double Probability { get; set; }
        public List<string> Recommendations { get; set; }
        public string Severity { get; set; }
    }

    /// <summary>
    /// Request para predicción ML
    /// </summary>
    public class MLPredictionRequest
    {
        public List<string> Symptoms { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }

    /// <summary>
    /// Response de predicción ML
    /// </summary>
    public class MLPredictionResponse
    {
        public string Disease { get; set; }
        public double Confidence { get; set; }
        public List<AlternativeDiagnosis> Alternatives { get; set; }
        public List<string> DecisionFactors { get; set; }
        public Dictionary<string, double> FeatureImportance { get; set; }
    }

    public class AlternativeDiagnosis
    {
        public string Disease { get; set; }
        public double Confidence { get; set; }
    }

    /// <summary>
    /// Request para chatbot
    /// </summary>
    public class ChatBotRequest
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public List<ChatMessage> ConversationHistory { get; set; }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// Response de chatbot
    /// </summary>
    public class ChatBotResponse
    {
        public string Response { get; set; }
        public string ConversationId { get; set; }
        public List<string> SuggestedActions { get; set; }
        public bool RequiresMedicalAttention { get; set; }
        public AIAnalysisResponse Analysis { get; set; }
    }

    /// <summary>
    /// Response de estado de salud del servicio
    /// </summary>
    public class AIHealthResponse
    {
        public string Status { get; set; }
        public string Version { get; set; }
        public Dictionary<string, object> Components { get; set; }
        public DateTime Timestamp { get; set; }
    }

    #endregion
}

