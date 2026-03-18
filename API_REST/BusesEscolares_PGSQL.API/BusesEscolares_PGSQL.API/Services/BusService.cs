using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using System.Drawing;


namespace BusesEscolares_PGSQL.API.Services
{
    public class BusService(IBusRepository busRepository)
    {
        private readonly IBusRepository _busRepository = busRepository;

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
    }
}
