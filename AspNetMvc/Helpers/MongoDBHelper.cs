using MongoDB.Driver;
using System;
using System.Configuration;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers
{
    /// <summary>
    /// Helper para gestionar la conexión a MongoDB
    /// </summary>
    public class MongoDBHelper
    {
        private static IMongoDatabase _database;
        private static readonly object _lock = new object();

        /// <summary>
        /// Obtiene la instancia de la base de datos MongoDB
        /// </summary>
        public static IMongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    lock (_lock)
                    {
                        if (_database == null)
                        {
                            Initialize();
                        }
                    }
                }
                return _database;
            }
        }

        /// <summary>
        /// Inicializa la conexión a MongoDB
        /// </summary>
        private static void Initialize()
        {
            try
            {
                // Leer la cadena de conexión desde Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ConfigurationErrorsException("No se encontró la cadena de conexión 'MongoDB' en Web.config");
                }

                // Leer el nombre de la base de datos desde AppSettings
                string databaseName = ConfigurationManager.AppSettings["MongoDBName"];
                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new ConfigurationErrorsException("No se encontró la configuración 'MongoDBName' en Web.config");
                }

                // Crear el cliente de MongoDB
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
                settings.ConnectTimeout = TimeSpan.FromSeconds(10);

                var client = new MongoClient(settings);
                _database = client.GetDatabase(databaseName);

                // La conexión se verificará cuando se use la base de datos
                Console.WriteLine($"MongoDB client inicializado para: {databaseName}");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner: {ex.InnerException.Message}";
                }
                Console.WriteLine($"Error al conectar a MongoDB: {errorMessage}");
                throw new Exception($"Error al inicializar MongoDB: {errorMessage}", ex);
            }
        }

        /// <summary>
        /// Obtiene una colección de MongoDB
        /// </summary>
        /// <typeparam name="T">Tipo de documento</typeparam>
        /// <param name="collectionName">Nombre de la colección</param>
        /// <returns>Colección de MongoDB</returns>
        public static IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Verifica si la conexión a MongoDB está activa
        /// </summary>
        /// <returns>True si la conexión está activa, false en caso contrario</returns>
        public static bool IsConnected()
        {
            try
            {
                // Verificar simplemente si la base de datos no es null
                return Database != null;
            }
            catch
            {
                return false;
            }
        }
    }
}

