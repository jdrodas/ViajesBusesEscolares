using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Services
{
    public class RutaService(IRutaRepository rutaRepository,
                            IZonaRepository zonaRepository,
                            IViajeRepository viajeRepository)
    {
        private readonly IRutaRepository _rutaRepository = rutaRepository;
        private readonly IZonaRepository _zonaRepository = zonaRepository;
        private readonly IViajeRepository _viajeRepository = viajeRepository;
        public async Task<List<Ruta>> GetAllAsync()
        {
            var lasRutas = await _rutaRepository
                .GetAllAsync();

            if (lasRutas.Count == 0)
                throw new EmptyCollectionException("No se encontraron rutas registradas");

            return lasRutas;
        }

        public async Task<Ruta> GetByIdAsync(string rutaId)
        {
            Ruta unaRuta = await _rutaRepository
                .GetByIdAsync(rutaId);

            if (unaRuta.Id == string.Empty)
                throw new EmptyCollectionException($"Ruta no encontrada con el Id {rutaId}");

            return unaRuta;
        }

        public async Task<Ruta> CreateAsync(Ruta unaRuta)
        {
            unaRuta.Nombre = unaRuta.Nombre!.Trim();
            unaRuta.ZonaNombre = unaRuta.ZonaNombre!.Trim();

            string resultadoValidacion = EvaluateRouteDetailsAsync(unaRuta);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var zonaExistente = await _zonaRepository
                .GetByNameAsync(unaRuta.ZonaNombre!);

            if (zonaExistente.Id == string.Empty)
                throw new EmptyCollectionException($"Zona para la ruta no encontrada con el nombre {unaRuta.ZonaNombre}");

            unaRuta.ZonaId = zonaExistente.Id;

            var rutaExistente = await _rutaRepository
                .GetByDetailsAsync(unaRuta);

            if (rutaExistente.Nombre == unaRuta.Nombre! &&
                rutaExistente.ZonaNombre == unaRuta.ZonaNombre! &&
                rutaExistente.DistanciaKms == unaRuta.DistanciaKms)
                return rutaExistente;

            try
            {
                bool resultadoAccion = await _rutaRepository
                    .CreateAsync(unaRuta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                rutaExistente = await _rutaRepository
                .GetByDetailsAsync(unaRuta);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return rutaExistente;
        }

        public async Task<Ruta> UpdateAsync(Ruta unaRuta)
        {
            unaRuta.Nombre = unaRuta.Nombre!.Trim();
            unaRuta.ZonaNombre = unaRuta.ZonaNombre!.Trim();

            string resultadoValidacion = EvaluateRouteDetailsAsync(unaRuta);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var zonaExistente = await _zonaRepository
                .GetByNameAsync(unaRuta.ZonaNombre!);

            if (zonaExistente.Id == string.Empty)
                throw new EmptyCollectionException($"Zona para la ruta no encontrada con el nombre {unaRuta.ZonaNombre}");

            unaRuta.ZonaId = zonaExistente.Id;

            var rutaExistente = await _rutaRepository
                .GetByIdAsync(unaRuta.Id!);

            if (rutaExistente.Id == string.Empty)
                throw new EmptyCollectionException($"No existe una ruta con el Id {unaRuta.Id} que se pueda actualizar");

            if (rutaExistente.Equals(unaRuta))
                return rutaExistente;

            try
            {
                bool resultadoAccion = await _rutaRepository
                    .UpdateAsync(unaRuta);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                rutaExistente = await _rutaRepository
                    .GetByIdAsync(unaRuta.Id!);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return rutaExistente;
        }

        public async Task<string> RemoveAsync(string rutaId)
        {
            Ruta unaRuta = await _rutaRepository
                .GetByIdAsync(rutaId);

            if (unaRuta.Id == string.Empty)
                throw new EmptyCollectionException($"Ruta no encontrada con el id {rutaId}");

            var viajesAsociados = await _viajeRepository
                .GetAssociatedTripsToRouteByIdAsync(rutaId);

            if (viajesAsociados.Count != 0)
                throw new AppValidationException($"La Ruta {unaRuta.Nombre} no se puede eliminar porque tiene {viajesAsociados.Count} viajes asociados");

            string nombreRutaEliminada = unaRuta.Nombre!;

            try
            {
                bool resultadoAccion = await _rutaRepository
                    .RemoveAsync(rutaId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreRutaEliminada;
        }



        private static string EvaluateRouteDetailsAsync(Ruta unaRuta)
        {
            if (string.IsNullOrEmpty(unaRuta.Nombre))
                return "No se puede insertar una ruta con nombre nulo";

            if (string.IsNullOrEmpty(unaRuta.ZonaNombre))
                return "No se puede insertar una ruta con sin nombre de zona";

            if (unaRuta.DistanciaKms <= 0)
                return "Las rutas deben tener una distancia en kms positiva";

            return string.Empty;
        }
    }
}
