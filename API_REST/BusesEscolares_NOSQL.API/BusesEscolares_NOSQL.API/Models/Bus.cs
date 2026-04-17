using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace BusesEscolares_NOSQL.API.Models
{
    public class Bus
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("placa")]
        [BsonElement("placa")]
        [BsonRepresentation(BsonType.String)]
        public string? Placa { get; set; } = string.Empty;

        [JsonPropertyName("año_fabricacion")]
        [BsonElement("año_fabricacion")]
        [BsonRepresentation(BsonType.Int32)]
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
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Placa?.GetHashCode() ?? 0);
                hash = hash * 5 + AñoFabricacion.GetHashCode();

                return hash;
            }
        }
    }
}