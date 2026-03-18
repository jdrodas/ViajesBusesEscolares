using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class Bus
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("placa")]
        public string? Placa { get; set; } = string.Empty;

        [JsonPropertyName("año_fabricacion")]
        public int AñoFabricacion { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroBus = (Bus)obj;

            return Id == otroBus.Id
                && Placa!.Equals(otroBus.Placa)
                && AñoFabricacion == otroBus.AñoFabricacion;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Placa?.GetHashCode() ?? 0);
                hash = hash * 5 + AñoFabricacion.GetHashCode();

                return hash;
            }
        }
    }
}