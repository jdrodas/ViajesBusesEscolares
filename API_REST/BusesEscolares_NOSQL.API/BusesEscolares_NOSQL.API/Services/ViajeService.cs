using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Services
{
    public class ViajeService(IViajeRepository viajeRepository)
    {
        private readonly IViajeRepository _viajeRepository = viajeRepository;

        public async Task<List<Viaje>> GetAllAsync()
        {
            var losViajes = await _viajeRepository
                .GetAllAsync();

            if (losViajes.Count == 0)
                throw new EmptyCollectionException("No se encontraron zonas registradas");

            return losViajes;
        }

        public async Task<Viaje> GetByIdAsync(string viajeId)
        {
            Viaje unViaje = await _viajeRepository
                .GetByIdAsync(viajeId);

            if (unViaje.Id == string.Empty)
                throw new EmptyCollectionException($"Viaje no encontrado con el Id {viajeId}");

            return unViaje;
        }
    }
}
