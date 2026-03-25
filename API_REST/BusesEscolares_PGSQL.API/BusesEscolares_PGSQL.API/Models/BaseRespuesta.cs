using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class BaseRespuesta
    {
        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; } = string.Empty;

        [JsonPropertyName("totalElementos")]
        public int TotalElementos { get; set; }

        [JsonPropertyName("pagina")]
        public int Pagina { get; set; }

        [JsonPropertyName("elementosPorPagina")]
        public int ElementosPorPagina { get; set; }

        [JsonPropertyName("totalPaginas")]
        public int TotalPaginas { get; set; }
    }
}
