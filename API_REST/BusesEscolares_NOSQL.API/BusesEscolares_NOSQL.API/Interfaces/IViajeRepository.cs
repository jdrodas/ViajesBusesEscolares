using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Interfaces
{
    public interface IViajeRepository
    {
        public Task<List<Viaje>> GetAllAsync();
        public Task<long> GetTotalAsync();
        public Task<Viaje> GetByIdAsync(string viajeId);

        //TODO: Declarar GetByDetailsAsync
        public Task<bool> UpdateBusDataAsync(Bus unBus);
        public Task<bool> UpdateRouteDataAsync(Ruta unaRuta);
        public Task<List<Viaje>> GetAssociatedTripsToBusByIdAsync(string busId);
        public Task<List<Viaje>> GetAssociatedTripsToRouteByIdAsync(string rutaId);
        public Task<List<Viaje>> GetAssociatedTripsToZoneByIdAsync(string zonaId);
    }
}