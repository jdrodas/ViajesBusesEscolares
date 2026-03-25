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

        public async Task<BusRespuesta> GetAllAsync(BusParametrosConsulta parametrosConsulta)
        {
            int totalElementos = await _busRepository
                .GetTotalAsync();

            int totalPaginas = (int)Math.Ceiling((double)totalElementos / parametrosConsulta.ElementosPorPagina);

            if (parametrosConsulta.Pagina <= 0)
                throw new AppValidationException($"El número de la página debe ser un valor positivo.");

            if (parametrosConsulta.Pagina > totalPaginas)
                throw new AppValidationException($"La página solicitada No. {parametrosConsulta.Pagina} excede el número total " +
                    $"de página de {totalPaginas} con una cantidad de elementos por página de {parametrosConsulta.ElementosPorPagina}");

            if (parametrosConsulta.ElementosPorPagina <= 0)
                throw new AppValidationException($"El número de elementos por página debe ser un valor positivo.");

            var losBuses = await _busRepository
                .GetAllAsync(parametrosConsulta);

            if (losBuses.Count == 0)
                throw new EmptyCollectionException("No se encontraron buses registrados");

            var respuestaBuses = BuildBusResponse(losBuses,
                                                    parametrosConsulta,
                                                    totalElementos,
                                                    totalPaginas);

            return respuestaBuses;
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

        private static BusRespuesta BuildBusResponse(List<Bus> losBuses,
                                                        BusParametrosConsulta parametrosConsulta,
                                                        int totalElementos,
                                                        int totalPaginas)
        {
            var respuestaBuses = new BusRespuesta
            {
                Tipo = "Buses",
                TotalElementos = totalElementos,
                Pagina = parametrosConsulta.Pagina,
                ElementosPorPagina = parametrosConsulta.ElementosPorPagina,
                TotalPaginas = totalPaginas,
                Data = [.. losBuses]
            };

            return respuestaBuses;
        }
    }
}
