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

        public async Task<List<Viaje>> GetAllAsync(ViajeParametrosConsulta parametrosConsulta)
        {
            var conexion = contextoDB.CreateConnection();

            //parametros de paginacion
            var desfase = (parametrosConsulta.Pagina - 1) * parametrosConsulta.ElementosPorPagina;

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@elementosPagina", parametrosConsulta.ElementosPorPagina,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@desfase", desfase,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL =
                @"SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, v.zona_id zonaId, v.zona_nombre zonaNombre, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "ORDER BY v.fecha_salida " +
                "LIMIT @elementosPagina " +
                "OFFSET @desfase";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoViajes];
        }

        public async Task<int> GetTotalAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.viajes";

            var totalViajes = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return totalViajes;
        }

        public async Task<Viaje> GetByIdAsync(Guid viajeId)
        {
            Viaje unViaje = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@viajeId", viajeId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, v.zona_id zonaId, v.zona_nombre zonaNombre, " +
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
                "v.total_pasajeros totalPasajeros, v.zona_id zonaId, v.zona_nombre zonaNombre, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "WHERE v.bus_id = @busId " +
                "ORDER BY v.ruta_nombre";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoViajes];
        }

        public async Task<List<Viaje>> GetAssociatedTripsToRouteByIdAsync(Guid rutaId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@rutaId", rutaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, v.zona_id zonaId, v.zona_nombre zonaNombre, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "WHERE v.ruta_id = @rutaId " +
                "ORDER BY v.bus_placa";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoViajes];
        }

        public async Task<List<Viaje>> GetAssociatedTripsToZoneByIdAsync(Guid zonaId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@zonaId", zonaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT v.viaje_id id, v.ruta_id rutaId, v.bus_id busId, " +
                "v.ruta_nombre rutaNombre, v.bus_placa busPlaca, v.viaje_turno turno, " +
                "v.total_pasajeros totalPasajeros, v.zona_id zonaId, v.zona_nombre zonaNombre, " +
                "to_char(v.fecha_salida,'DD/MM/YYYY HH24:MI:SS') fechaSalida, " +
                "to_char(v.fecha_llegada,'DD/MM/YYYY HH24:MI:SS') fechaLlegada " +
                "FROM core.v_info_viajes v " +
                "WHERE v.zona_id = @zonaId " +
                "ORDER BY v.ruta_nombre";

            var resultadoViajes = await conexion
                .QueryAsync<Viaje>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoViajes];
        }
    }
}
