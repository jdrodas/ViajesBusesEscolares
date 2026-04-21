using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Interfaces
{
    public interface IViajeRepository
    {
        public Task<List<Viaje>> GetAllAsync();
        //public Task<int> GetTotalAsync();
        //public Task<Viaje> GetByIdAsync(string viajeId);
        public Task<List<Viaje>> GetAssociatedTripsToBusByIdAsync(string busId);
        public Task<List<Viaje>> GetAssociatedTripsToRouteByIdAsync(string rutaId);
        public Task<List<Viaje>> GetAssociatedTripsToZoneByIdAsync(string zonaId);
    }
}