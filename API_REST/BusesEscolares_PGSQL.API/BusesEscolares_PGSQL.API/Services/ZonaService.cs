using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using BusesEscolares_PGSQL.API.Repositories;

namespace BusesEscolares_PGSQL.API.Services
{
    public class ZonaService(IZonaRepository zonaRepository,
                             IViajeRepository viajeRepository)
    {
        private readonly IZonaRepository _zonaRepository = zonaRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;

        public async Task<ZonaRespuesta> GetAllAsync(ZonaParametrosConsulta parametrosConsulta)
        {
            int totalElementos = await _zonaRepository
                .GetTotalAsync();

            int totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsulta.ElementosPorPagina);

            if (parametrosConsulta.Pagina <= 0)
                throw new AppValidationException($"El número de la página debe ser un valor positivo.");

            if (parametrosConsulta.Pagina > totalPaginas)
                throw new AppValidationException($"La página solicitada No. {parametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {parametrosConsulta.ElementosPorPagina}");

            if (parametrosConsulta.ElementosPorPagina <= 0)
                throw new AppValidationException($"El número de elementos por página debe ser un valor positivo.");

            var lasZonas = await _zonaRepository
                .GetAllAsync(parametrosConsulta);

            if (lasZonas.Count == 0)
                throw new EmptyCollectionException("No se encontraron zonas registradas");

            var respuestaZonas = BuildZoneResponse(lasZonas,
                                                    parametrosConsulta,
                                                    totalElementos,
                                                    totalPaginas);

            return respuestaZonas;
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

        private static ZonaRespuesta BuildZoneResponse(List<Zona> lasZonas,
                                                        ZonaParametrosConsulta parametrosConsulta,
                                                        int totalElementos,
                                                        int totalPaginas)
        {
            var respuestaZonas = new ZonaRespuesta
            {
                Tipo = "Zonas",
                TotalElementos = totalElementos,
                Pagina = parametrosConsulta.Pagina,
                ElementosPorPagina = parametrosConsulta.ElementosPorPagina,
                TotalPaginas = totalPaginas,
                Data = [.. lasZonas]
            };

            return respuestaZonas;
        }
    }
}
