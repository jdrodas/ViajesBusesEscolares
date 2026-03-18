using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class Ruta
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("distancia_kms")]
        public float? DistanciaKms { get; set; } = 0.0f;

        [JsonPropertyName("zona_nombre")]
        public string? ZonaNombre { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraRuta = (Ruta)obj;

            return Id == otraRuta.Id
                && Nombre!.Equals(otraRuta.Nombre)
                && ZonaNombre!.Equals(otraRuta.ZonaNombre)
                && DistanciaKms == otraRuta.DistanciaKms;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (ZonaNombre?.GetHashCode() ?? 0);
                hash = hash * 5 + DistanciaKms.GetHashCode();

                return hash;
            }
        }
    }
}


