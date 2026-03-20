using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;


namespace BusesEscolares_PGSQL.API.Services
{
    public class BusService(IBusRepository busRepository,
                            IViajeRepository viajeRepository)
    {
        private readonly IBusRepository _busRepository = busRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;

        public async Task<List<Bus>> GetAllAsync()
        {
            var losBuses = await _busRepository
                .GetAllAsync();

            if (losBuses.Count == 0)
                throw new EmptyCollectionException("No se encontraron buses registrados");

            return losBuses;
        }

        public async Task<Bus> GetByIdAsync(Guid busId)
        {
            Bus unBus = await _busRepository
                .GetByIdAsync(busId);

            if (unBus.Id == Guid.Empty)
                throw new EmptyCollectionException($"Bus no encontrado con el Id {busId}");

            return unBus;
        }

        public async Task<List<Viaje>> GetAssociatedTripsByIdAsync(Guid busId)
        {
            Bus unBus = await _busRepository
                .GetByIdAsync(busId);

            if (unBus.Id == Guid.Empty)
                throw new EmptyCollectionException($"Bus no encontrado con el Id {busId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToBusByIdAsync(busId);

            if (viajesAsociados.Count == 0)
                throw new EmptyCollectionException($"No se encontraron viajes asociados al bus con placa {unBus.Placa}");

            return viajesAsociados;
        }
    }
}
