using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using BusesEscolares_PGSQL.API.Repositories;

namespace BusesEscolares_PGSQL.API.Services
{
    public class ViajeService(IViajeRepository viajeRepository)
    {
        private readonly IViajeRepository _viajeRepository = viajeRepository;
        public async Task<List<Viaje>> GetAllAsync()
        {
            var losViajes = await _viajeRepository
                .GetAllAsync();

            if (losViajes.Count == 0)
                throw new EmptyCollectionException("No se encontraron viajes registrados");

            return losViajes;
        }

        public async Task<Viaje> GetByIdAsync(Guid viajeId)
        {
            Viaje unViaje = await _viajeRepository
                .GetByIdAsync(viajeId);

            if (unViaje.Id == Guid.Empty)
                throw new EmptyCollectionException($"Viaje no encontrada con el Id {viajeId}");

            return unViaje;
        }
    }
}
