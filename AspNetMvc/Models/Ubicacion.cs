using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    /// <summary>
    /// Modelo de Ubicación Geográfica
    /// </summary>
    public class Ubicacion
    {
        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
        [Display(Name = "Latitud")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
        [Display(Name = "Longitud")]
        public double? Longitude { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        // Propiedades calculadas
        [BsonIgnore]
        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

        [BsonIgnore]
        public string CoordinatesText => HasCoordinates ? $"{Latitude}, {Longitude}" : "Sin coordenadas";
    }
}

