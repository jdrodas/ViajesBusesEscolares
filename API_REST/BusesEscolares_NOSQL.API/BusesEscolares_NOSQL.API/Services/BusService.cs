using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Services
{
    public class BusService(IBusRepository busRepository,
                        IViajeRepository viajeRepository)
    {
        private readonly IBusRepository _busRepository = busRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;

        public async Task<List<Bus>> GetAllAsync()
        {
            var losBuses = await _busRepository
                .GetAllAsync();

            if (losBuses.Count == 0)
                throw new EmptyCollectionException("No se encontraron buses registrados");

            return losBuses;
        }

        public async Task<Bus> GetByIdAsync(string busId)
        {
            Bus unBus = await _busRepository
                .GetByIdAsync(busId);

            if (unBus.Id == string.Empty)
                throw new EmptyCollectionException($"Bus no encontrado con el Id {busId}");

            return unBus;
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
                .GetByIdAsync(unBus.Id!);

            if (busExistente.Id == string.Empty)
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
                    .GetByIdAsync(unBus.Id!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return busExistente;
        }

        public async Task<string> RemoveAsync(string busId)
        {
            Bus unBus = await _busRepository
                .GetByIdAsync(busId);

            if (unBus.Id == string.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el id {busId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToBusByIdAsync(busId);

            if (viajesAsociados.Count != 0)
                throw new AppValidationException($"El bus de placa {unBus.Placa} no se puede eliminar porque tiene {viajesAsociados.Count} viajes asociados");

            string placaBusEliminado = unBus.Placa!;

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

            return placaBusEliminado;
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
