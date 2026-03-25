using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class RutaRespuesta : BaseRespuesta
    {
        [JsonPropertyName("data")]
        public List<Ruta> Data { get; set; } = [];
    }
}
