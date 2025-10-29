using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers
{
    /// <summary>
    /// Helper para realizar peticiones HTTP a los servicios de IA
    /// </summary>
    public class HttpClientHelper
    {
        private static readonly HttpClient _httpClient;
        private static readonly string _aiServiceUrl;

        static HttpClientHelper()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };

            _aiServiceUrl = System.Configuration.ConfigurationManager.AppSettings["AIServiceUrl"] ?? "http://localhost:8000";
        }

        /// <summary>
        /// Realiza una petición GET a la API de IA
        /// </summary>
        /// <typeparam name="T">Tipo de respuesta esperada</typeparam>
        /// <param name="endpoint">Endpoint de la API</param>
        /// <returns>Respuesta deserializada</returns>
        public static async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var url = $"{_aiServiceUrl}{endpoint}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar petición GET a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición POST a la API de IA
        /// </summary>
        /// <typeparam name="TRequest">Tipo de request</typeparam>
        /// <typeparam name="TResponse">Tipo de respuesta esperada</typeparam>
        /// <param name="endpoint">Endpoint de la API</param>
        /// <param name="data">Datos a enviar</param>
        /// <returns>Respuesta deserializada</returns>
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var url = $"{_aiServiceUrl}{endpoint}";
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar petición POST a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición PUT a la API de IA
        /// </summary>
        /// <typeparam name="TRequest">Tipo de request</typeparam>
        /// <typeparam name="TResponse">Tipo de respuesta esperada</typeparam>
        /// <param name="endpoint">Endpoint de la API</param>
        /// <param name="data">Datos a enviar</param>
        /// <returns>Respuesta deserializada</returns>
        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var url = $"{_aiServiceUrl}{endpoint}";
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar petición PUT a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición DELETE a la API de IA
        /// </summary>
        /// <param name="endpoint">Endpoint de la API</param>
        /// <returns>Task</returns>
        public static async Task DeleteAsync(string endpoint)
        {
            try
            {
                var url = $"{_aiServiceUrl}{endpoint}";
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar petición DELETE a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica si el servicio de IA está disponible
        /// </summary>
        /// <returns>True si está disponible, false en caso contrario</returns>
        public static async Task<bool> IsAIServiceAvailable()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_aiServiceUrl}/api/v1/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}

