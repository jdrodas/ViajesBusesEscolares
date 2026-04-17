using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace BusesEscolares_NOSQL.API.Models
{
    public class Zona
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
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
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
