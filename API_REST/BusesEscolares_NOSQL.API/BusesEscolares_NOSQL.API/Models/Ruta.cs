using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace BusesEscolares_NOSQL.API.Models
{
    public class Ruta
    {

        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("distancia_kms")]
        [BsonElement("distancia_kms")]
        [BsonRepresentation(BsonType.Double)]
        public double? DistanciaKms { get; set; } = 0.0d;

        [JsonPropertyName("zona_id")]
        [BsonElement("zona_id")]
        [BsonRepresentation(BsonType.String)]
        public string? ZonaId { get; set; } = string.Empty;

        [JsonPropertyName("zona_nombre")]
        [BsonElement("zona_nombre")]
        [BsonRepresentation(BsonType.String)]
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
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (ZonaNombre?.GetHashCode() ?? 0);
                hash = hash * 5 + DistanciaKms.GetHashCode();

                return hash;
            }
        }
    }
}


