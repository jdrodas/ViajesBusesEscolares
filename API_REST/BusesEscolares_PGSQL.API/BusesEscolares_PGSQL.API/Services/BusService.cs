using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using System.Drawing;


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

        public async Task<Bus> CreateAsync(Bus unBus)
        {
            unBus.Placa = unBus.Placa!.Trim();

            string resultadoValidacion = EvaluateBusDetailsAsync(unBus);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var busExistente = await _busRepository
                .GetByDetailsAsync(unBus);

            if (busExistente.Placa == unBus.Placa! &&
                busExistente.AñoFabricacion == unBus.AñoFabricacion)
                return busExistente;

            try
            {
                bool resultadoAccion = await _busRepository
                    .CreateAsync(unBus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                busExistente = await _busRepository
                .GetByDetailsAsync(unBus);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return busExistente;
        }

        public async Task<Bus> UpdateAsync(Bus unBus)
        {
            unBus.Placa = unBus.Placa!.Trim();

            string resultadoValidacion = EvaluateBusDetailsAsync(unBus);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var busExistente = await _busRepository
                .GetByIdAsync(unBus.Id);

            if (busExistente.Id == Guid.Empty)
                throw new EmptyCollectionException($"No existe un bus con el Guid {unBus.Id} que se pueda actualizar");

            if (busExistente.Equals(unBus))
                return busExistente;

            try
            {
                bool resultadoAccion = await _busRepository
                    .UpdateAsync(unBus);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                busExistente = await _busRepository
                    .GetByIdAsync(unBus.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return busExistente;
        }

        public async Task<string> RemoveAsync(Guid busId)
        {
            Bus unBus = await _busRepository
                .GetByIdAsync(busId);

            if (unBus.Id == Guid.Empty)
                throw new EmptyCollectionException($"Bus no encontrado con el id {busId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToBusByIdAsync(busId);

            if (viajesAsociados.Count != 0)
                throw new AppValidationException($"El bus de placa {unBus.Placa} no se puede eliminar porque tiene {viajesAsociados.Count} viajes asociados");

            string placaBusEliimnado = unBus.Placa!;

            try
            {
                bool resultadoAccion = await _busRepository
                    .RemoveAsync(busId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return placaBusEliimnado;
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

        private static string EvaluateBusDetailsAsync(Bus unBus)
        {
            if (string.IsNullOrEmpty(unBus.Placa))
                return "No se puede insertar un bus con placa nula";

            if (unBus.AñoFabricacion <= 2000)
                return "El año de fabricación del bus debe ser superior al año 2000";

            return string.Empty;
        }
    }
}
