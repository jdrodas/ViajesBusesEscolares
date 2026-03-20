using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class ViajeRepository(PgsqlDbContext unContexto) : IViajeRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;
        
        public async Task<List<Viaje>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v ";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoViajes];
        }

        public async Task<Viaje> GetByIdAsync(Guid viajeId)
        {
            Viaje unViaje= new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@viajeId", viajeId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "WHERE v.viaje_id = @viajeId";

            var resultado = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unViaje = resultado.First();

            return unViaje;
        }

        public async Task<List<Viaje>> GetAssociatedTripsToBusByIdAsync(Guid busId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@busId", busId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "WHERE v.bus_id = @busId " +
                "ORDER BY v.ruta_nombre";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoViajes];
        }
    }
}
