using System.Text.Json.Serialization;

namespace BusesEscolares_NOSQL.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("totalBuses")]
        public long Buses { get; set; } = 0;

        [JsonPropertyName("totalViajes")]
        public long Viajes { get; set; } = 0;

        [JsonPropertyName("TotalRutas")]
        public long Rutas { get; set; } = 0;

        [JsonPropertyName("totalZonas")]
        public long Zonas { get; set; } = 0;
    }
}