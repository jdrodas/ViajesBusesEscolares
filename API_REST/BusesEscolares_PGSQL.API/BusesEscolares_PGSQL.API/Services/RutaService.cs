using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Services
{
    public class RutaService(IRutaRepository rutaRepository,
                             IViajeRepository viajeRepository)
    {
        private readonly IRutaRepository _rutaRepository = rutaRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;
        public async Task<List<Ruta>> GetAllAsync()
        {
            var lasRutas = await _rutaRepository
                .GetAllAsync();

            if (lasRutas.Count == 0)
                throw new EmptyCollectionException("No se encontraron rutas registradas");

            return lasRutas;
        }

        public async Task<Ruta> GetByIdAsync(Guid rutaId)
        {
            Ruta unaRuta = await _rutaRepository
                .GetByIdAsync(rutaId);

            if (unaRuta.Id == Guid.Empty)
                throw new EmptyCollectionException($"Ruta no encontrada con el Id {rutaId}");

            return unaRuta;
        }

        public async Task<List<Viaje>> GetAssociatedTripsByIdAsync(Guid rutaId)
        {
            Ruta unaRuta = await _rutaRepository
                .GetByIdAsync(rutaId);

            if (unaRuta.Id == Guid.Empty)
                throw new EmptyCollectionException($"Ruta no encontrada con el Id {rutaId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToRouteByIdAsync(rutaId);

            if (viajesAsociados.Count == 0)
                throw new EmptyCollectionException($"No se encontraron viajes asociados a la ruta {unaRuta.Nombre}");

            return viajesAsociados;
        }
    }
}
