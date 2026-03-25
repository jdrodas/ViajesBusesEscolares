using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class ZonaRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Zona> Data { get; set; } = [];
    }
}