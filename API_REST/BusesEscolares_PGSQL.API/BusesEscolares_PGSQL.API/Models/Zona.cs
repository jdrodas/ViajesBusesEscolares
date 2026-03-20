using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class Zona
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraZona = (Zona)obj;

            return Id == otraZona.Id
                && Nombre!.Equals(otraZona.Nombre);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
