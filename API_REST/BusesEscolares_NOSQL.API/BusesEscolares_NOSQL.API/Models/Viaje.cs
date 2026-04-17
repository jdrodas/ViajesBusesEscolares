using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace BusesEscolares_NOSQL.API.Models
{
    public class Viaje
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ruta_id")]
        [BsonElement("ruta_id")]
        [BsonRepresentation(BsonType.String)]
        public string RutaId { get; set; } = string.Empty;

        [JsonPropertyName("ruta_nombre")]
        [BsonElement("ruta_nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? RutaNombre { get; set; } = string.Empty;

        [JsonPropertyName("zona_id")]
        [BsonElement("zona_id")]
        [BsonRepresentation(BsonType.String)]
        public string zonaId { get; set; } = string.Empty;

        [JsonPropertyName("zona_nombre")]
        [BsonElement("zona_nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? ZonaNombre { get; set; } = string.Empty;

        [JsonPropertyName("bus_id")]
        [BsonElement("bus_id")]
        [BsonRepresentation(BsonType.String)]
        public string BusId { get; set; } = string.Empty;

        [JsonPropertyName("bus_placa")]
        [BsonElement("bus_placa")]
        [BsonRepresentation(BsonType.String)]
        public string? BusPlaca { get; set; } = string.Empty;

        [JsonPropertyName("turno")]
        [BsonElement("turno")]
        [BsonRepresentation(BsonType.String)]
        public string? Turno { get; set; } = string.Empty;

        [JsonPropertyName("total_pasajeros")]
        [BsonElement("total_pasajeros")]
        [BsonRepresentation(BsonType.Int32)]
        public int TotalPasajeros { get; set; } = 0;

        [JsonPropertyName("fecha_salida")]
        [BsonElement("fecha_salida")]
        [BsonRepresentation(BsonType.String)]
        public string? FechaSalida { get; set; } = string.Empty;

        [JsonPropertyName("fecha_llegada")]
        [BsonElement("fecha_llegada")]
        [BsonRepresentation(BsonType.String)]
        public string? FechaLlegada { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroViaje = (Viaje)obj;

            return Id == otroViaje.Id
                && RutaId!.Equals(otroViaje.RutaId)
                && BusId!.Equals(otroViaje.BusId)
                && Turno!.Equals(otroViaje.Turno)
                && TotalPasajeros == otroViaje.TotalPasajeros
                && FechaSalida!.Equals(otroViaje.FechaSalida)
                && FechaLlegada!.Equals(otroViaje.FechaLlegada);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (RutaId?.GetHashCode() ?? 0);
                hash = hash * 5 + (BusId?.GetHashCode() ?? 0);
                hash = hash * 5 + (Turno?.GetHashCode() ?? 0);
                hash = hash * 5 + (FechaSalida?.GetHashCode() ?? 0);
                hash = hash * 5 + (FechaLlegada?.GetHashCode() ?? 0);
                hash = hash * 5 + TotalPasajeros.GetHashCode();

                return hash;
            }
        }
    }
}


