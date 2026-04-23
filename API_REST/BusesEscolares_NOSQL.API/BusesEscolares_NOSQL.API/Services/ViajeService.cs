using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using BusesEscolares_NOSQL.API.Repositories;
using System.Globalization;

namespace BusesEscolares_NOSQL.API.Services
{
    public class ViajeService(IViajeRepository viajeRepository,
                              IZonaRepository zonaRepository,
                              IRutaRepository rutaRepository,
                              IBusRepository busRepository)
    {
        private readonly IViajeRepository _viajeRepository = viajeRepository;
        private readonly IZonaRepository _zonaRepository = zonaRepository;
        private readonly IRutaRepository _rutaRepository = rutaRepository;
        private readonly IBusRepository _busRepository = busRepository;

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

        public async Task<Viaje> CreateAsync(Viaje unViaje)
        {
            string resultadoValidacion = EvaluateTripDetailsAsync(unViaje);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var zonaExistente = await _zonaRepository
                .GetByNameAsync(unViaje.ZonaNombre!);

            if (zonaExistente.Id == string.Empty)
                throw new EmptyCollectionException($"Zona para la ruta no encontrada con el nombre {unaRuta.ZonaNombre}");

            unViaje.ZonaId = zonaExistente.Id!;
            unViaje.ZonaNombre = zonaExistente.Nombre;

            var rutaExistente = await _rutaRepository
                .GetByNameAsync(unViaje.RutaNombre!);

            if (rutaExistente.Id == string.Empty)
                throw new EmptyCollectionException($"Ruta para el viaje no encontrada con el nombre {unViaje.RutaNombre}");

            unViaje.RutaId = rutaExistente.Id!;
            unViaje.RutaNombre = rutaExistente.Nombre;

            var busExistente = await _busRepository
                .GetByLicensePlateAsync(unViaje.BusPlaca!);

            if (busExistente.Id == string.Empty)
                throw new EmptyCollectionException($"Bus para el viaje no encontrado con la placa {unViaje.BusPlaca}");

            unViaje.BusId = busExistente.Id!;
            unViaje.BusPlaca = busExistente.Placa;

            //TODO: Validar que no exista un viaje con estos datos.

            try
            {
                bool resultadoAccion = await _viajeRepository
                    .CreateAsync(unViaje);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                var viajeExistente = await _viajeRepository
                .GetByDetailsAsync(unaRuta);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return rutaExistente;
        }

        private static string EvaluateTripDetailsAsync(Viaje unViaje)
        {
            unViaje.RutaNombre = unViaje.RutaNombre!.Trim();
            unViaje.ZonaNombre = unViaje.ZonaNombre!.Trim();
            unViaje.Turno = unViaje.Turno!.Trim();
            unViaje.FechaLlegada = unViaje.FechaLlegada!.Trim();
            unViaje.FechaSalida = unViaje.FechaSalida!.Trim();
            unViaje.BusPlaca = unViaje.BusPlaca!.Trim();

            if (string.IsNullOrEmpty(unViaje.RutaNombre))
                return "No se puede insertar un viaje con nombre de ruta nulo";

            if (string.IsNullOrEmpty(unViaje.ZonaNombre))
                return "No se puede insertar un viaje con nombre de zona nulo";

            if (string.IsNullOrEmpty(unViaje.Turno))
                return "No se puede insertar un viaje con turno nulo";

            if (string.IsNullOrEmpty(unViaje.FechaSalida)) 
                return "No se puede insertar un viaje con fecha de salida nula";

            if (string.IsNullOrEmpty(unViaje.FechaLlegada))
                return "No se puede insertar un viaje con fecha de llegada nula";

            if (string.IsNullOrEmpty(unViaje.BusPlaca)) 
                return "No se puede insertar un viaje con placa de bus nula";

            if (unViaje.TotalPasajeros <= 0)
                return "Los viajes deben tener una cantidad de pasajeros positiva";

            bool fechaValida = DateTime
                            .TryParseExact(
                                unViaje.FechaSalida, "yyyy-MM-dd HH:mm:ss",
                                CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out DateTime fechaSalidaValidada);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {unViaje.FechaSalida} no tiene el formato yyyy-MM-dd HH:mm:ss");

            fechaValida = DateTime
                            .TryParseExact(
                                unViaje.FechaLlegada, "yyyy-MM-dd HH:mm:ss",
                                CultureInfo.InvariantCulture, DateTimeStyles.None,
                                out DateTime fechaLlegadValidada);

            if (!fechaValida)
                throw new AppValidationException($"La fecha suministrada {unViaje.FechaSalida} no tiene el formato yyyy-MM-dd HH:mm:ss");

            if(fechaLlegadValidada < fechaSalidaValidada)
                throw new AppValidationException($"Las fechas suministradas no delimitan un lapso. Fecha llegada debe ser mayor que Fecha Salida");

            var duracion_minutos = fechaLlegadValidada - fechaSalidaValidada;

            unViaje.DuracionMinutos = duracion_minutos.Minutes;

            return string.Empty;
        }
    }
}
