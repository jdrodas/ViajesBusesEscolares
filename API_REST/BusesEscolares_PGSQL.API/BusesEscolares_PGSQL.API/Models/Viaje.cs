using System.Text.Json.Serialization;

namespace BusesEscolares_PGSQL.API.Models
{
    public class Viaje
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("ruta_id")]
        public Guid RutaId { get; set; } = Guid.Empty;

        [JsonPropertyName("ruta_nombre")]
        public string? RutaNombre { get; set; } = string.Empty;

        [JsonPropertyName("bus_id")]
        public Guid BusId { get; set; } = Guid.Empty;

        [JsonPropertyName("bus_placa")]
        public string? BusPlaca { get; set; } = string.Empty;

        [JsonPropertyName("turno")]
        public string? Turno { get; set; } = string.Empty;

        [JsonPropertyName("total_pasajeros")]
        public int TotalPasajeros { get; set; } = 0;

        [JsonPropertyName("fecha_salida")]
        public string? FechaSalida { get; set; } = string.Empty;

        [JsonPropertyName("fecha_llegada")]
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
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + RutaId.GetHashCode();
                hash = hash * 5 + BusId.GetHashCode();
                hash = hash * 5 + (Turno?.GetHashCode() ?? 0);
                hash = hash * 5 + (FechaSalida?.GetHashCode() ?? 0);
                hash = hash * 5 + (FechaLlegada?.GetHashCode() ?? 0);
                hash = hash * 5 + TotalPasajeros.GetHashCode();

                return hash;
            }
        }
    }
}


