using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Services
{
    public class ViajeService(IViajeRepository viajeRepository)
    {
        private readonly IViajeRepository _viajeRepository = viajeRepository;
        public async Task<ViajeRespuesta> GetAllAsync(ViajeParametrosConsulta parametrosConsulta)
        {
            int totalElementos = await _viajeRepository
                .GetTotalAsync();

            int totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsulta.ElementosPorPagina);

            if (parametrosConsulta.Pagina <= 0)
                throw new AppValidationException($"El número de la página debe ser un valor positivo.");

            if (parametrosConsulta.Pagina > totalPaginas)
                throw new AppValidationException($"La página solicitada No. {parametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {parametrosConsulta.ElementosPorPagina}");

            if (parametrosConsulta.ElementosPorPagina <= 0)
                throw new AppValidationException($"El número de elementos por página debe ser un valor positivo.");

            var losViajes = await _viajeRepository
                .GetAllAsync(parametrosConsulta);

            if (losViajes.Count == 0)
                throw new EmptyCollectionException("No se encontraron viajes registrados");

            var respuestaViajes = BuildTripResponse(losViajes,
                                                    parametrosConsulta,
                                                    totalElementos,
                                                    totalPaginas);

            return respuestaViajes;
        }

        public async Task<Viaje> GetByIdAsync(Guid viajeId)
        {
            Viaje unViaje = await _viajeRepository
                .GetByIdAsync(viajeId);

            if (unViaje.Id == Guid.Empty)
                throw new EmptyCollectionException($"Viaje no encontrada con el Id {viajeId}");

            return unViaje;
        }

        private static ViajeRespuesta BuildTripResponse(List<Viaje> losViajes,
                                                        ViajeParametrosConsulta parametrosConsulta,
                                                        int totalElementos,
                                                        int totalPaginas)
        {
            var respuestaViajes = new ViajeRespuesta
            {
                Tipo = "Viajes",
                TotalElementos = totalElementos,
                Pagina = parametrosConsulta.Pagina,
                ElementosPorPagina = parametrosConsulta.ElementosPorPagina,
                TotalPaginas = totalPaginas,
                Data = [.. losViajes]
            };

            return respuestaViajes;
        }
    }
}
