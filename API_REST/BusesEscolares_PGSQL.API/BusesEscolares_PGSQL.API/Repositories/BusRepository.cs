using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class BusRepository(PgsqlDbContext unContexto) : IBusRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;
        public async Task<List<Bus>> GetAllAsync(BusParametrosConsulta parametrosConsulta)
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
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses " +
                "ORDER BY placa " +
                "LIMIT @elementosPagina " +
                "OFFSET @desfase";

            var resultadoBuses = await conexion
                .QueryAsync<Bus>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoBuses];
        }

        public async Task<int> GetTotalAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.buses";

            var totalBuses = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return totalBuses;
        }

        public async Task<Bus> GetByIdAsync(Guid busId)
        {
            Bus unBus = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@busId", busId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses " +
                "WHERE id = @busId";

            var resultado = await conexion
                .QueryAsync<Bus>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unBus = resultado.First();

            return unBus;
        }

        public async Task<Bus> GetByDetailsAsync(Bus unBus)
        {
            Bus busExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@busPlaca", unBus.Placa,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@busAñoFabricacion", unBus.AñoFabricacion,
                        DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses " +
                "WHERE LOWER(placa) = LOWER(@busPlaca) " +
                "AND año_fabricacion = @busAñoFabricacion";

            var resultado = await conexion
                .QueryAsync<Bus>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                busExistente = resultado.First();

            return busExistente;
        }

        public async Task<bool> CreateAsync(Bus unBus)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_bus";
                var parametros = new
                {
                    p_placa = unBus.Placa,
                    p_año_fabricacion = unBus.AñoFabricacion
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Bus unBus)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_bus";
                var parametros = new
                {
                    p_id = unBus.Id,
                    p_placa = unBus.Placa,
                    p_año_fabricacion = unBus.AñoFabricacion
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(Guid busId)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_bus";
                var parametros = new
                {
                    p_id = busId
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
