using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IViajeRepository
    {
        public Task<List<Viaje>> GetAllAsync();
        public Task<Viaje> GetByIdAsync(Guid viajeId);
        public Task<List<Viaje>> GetAssociatedTripsToBusByIdAsync(Guid busId);
        public Task<List<Viaje>> GetAssociatedTripsToRouteByIdAsync(Guid rutaId);
        public Task<List<Viaje>> GetAssociatedTripsToZoneByIdAsync(Guid zonaId);
    }
}

