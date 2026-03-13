using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("zonas")]
        public long Zonas { get; set; } = 0;

        [JsonPropertyName("rutas")]
        public long Rutas { get; set; } = 0;

        [JsonPropertyName("buses")]
        public long Buses { get; set; } = 0;

        [JsonPropertyName("viajes")]
        public long Viajes { get; set; } = 0;

    }
}

