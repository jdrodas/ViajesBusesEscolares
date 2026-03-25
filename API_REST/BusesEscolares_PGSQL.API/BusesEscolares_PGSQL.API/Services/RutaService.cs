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
        public async Task<RutaRespuesta> GetAllAsync(RutaParametrosConsulta parametrosConsulta)
        {
            int totalElementos = await _rutaRepository
                .GetTotalAsync();

            int totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsulta.ElementosPorPagina);

            if (parametrosConsulta.Pagina <= 0)
                throw new AppValidationException($"El número de la página debe ser un valor positivo.");

            if (parametrosConsulta.Pagina > totalPaginas)
                throw new AppValidationException($"La página solicitada No. {parametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {parametrosConsulta.ElementosPorPagina}");

            if (parametrosConsulta.ElementosPorPagina <= 0)
                throw new AppValidationException($"El número de elementos por página debe ser un valor positivo.");

            var lasRutas = await _rutaRepository
                .GetAllAsync(parametrosConsulta);

            if (lasRutas.Count == 0)
                throw new EmptyCollectionException("No se encontraron rutas registradas");

            var respuestaRutas = BuildRouteResponse(lasRutas,
                                                    parametrosConsulta,
                                                    totalElementos,
                                                    totalPaginas);

            return respuestaRutas;
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

        private static RutaRespuesta BuildRouteResponse(List<Ruta> lasRutas,
                                                        RutaParametrosConsulta parametrosConsulta,
                                                        int totalElementos,
                                                        int totalPaginas)
        {
            var respuestaRutas = new RutaRespuesta
            {
                Tipo = "Rutas",
                TotalElementos = totalElementos,
                Pagina = parametrosConsulta.Pagina,
                ElementosPorPagina = parametrosConsulta.ElementosPorPagina,
                TotalPaginas = totalPaginas,
                Data = [.. lasRutas]
            };

            return respuestaRutas;
        }
    }
}
