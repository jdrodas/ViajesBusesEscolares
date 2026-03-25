using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class ViajeRespuesta:BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Viaje> Data { get; set; } = [];
    }
}
