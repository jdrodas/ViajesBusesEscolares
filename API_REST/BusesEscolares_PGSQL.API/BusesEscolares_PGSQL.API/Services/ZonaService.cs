using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Services
{
    public class ZonaService(IZonaRepository zonaRepository,
                             IViajeRepository viajeRepository)
    {
        private readonly IZonaRepository _zonaRepository = zonaRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;

        public async Task<List<Zona>> GetAllAsync()
        {
            var lasZonas = await _zonaRepository
                .GetAllAsync();

            if (lasZonas.Count == 0)
                throw new EmptyCollectionException("No se encontraron zonas registradas");

            return lasZonas;
        }

        public async Task<Zona> GetByIdAsync(Guid zonaId)
        {
            Zona unaZona = await _zonaRepository
                .GetByIdAsync(zonaId);

            if (unaZona.Id == Guid.Empty)
                throw new EmptyCollectionException($"Zona no encontrada con el Id {zonaId}");

            return unaZona;
        }

        public async Task<List<Viaje>> GetAssociatedTripsByIdAsync(Guid zonaId)
        {
            Zona unaZona = await _zonaRepository
                .GetByIdAsync(zonaId);

            if (unaZona.Id == Guid.Empty)
                throw new EmptyCollectionException($"Zona no encontrada con el Id {zonaId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToZoneByIdAsync(zonaId);

            if (viajesAsociados.Count == 0)
                throw new EmptyCollectionException($"No se encontraron viajes asociados a la zona {unaZona.Nombre}");

            return viajesAsociados;
        }
    }
}
