using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class BusRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Bus> Data { get; set; } = [];
    }
}
