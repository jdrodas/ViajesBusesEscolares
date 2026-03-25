using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class ZonaRepository(PgsqlDbContext unContexto) : IZonaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Zona>> GetAllAsync(ZonaParametrosConsulta parametrosConsulta)
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
                "SELECT DISTINCT id, nombre " +
                "FROM core.zonas " +
                "ORDER BY nombre " +
                "LIMIT @elementosPagina " +
                "OFFSET @desfase";

            var resultadoZonas = await conexion
                .QueryAsync<Zona>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoZonas];
        }

        public async Task<int> GetTotalAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.zonas";

            var totalZonas = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return totalZonas;
        }

        public async Task<Zona> GetByIdAsync(Guid zonaId)
        {
            Zona unaZona = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@zonaId", zonaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre " +
                "FROM core.zonas " +
                "WHERE id = @zonaId";

            var resultado = await conexion
                .QueryAsync<Zona>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaZona = resultado.First();

            return unaZona;
        }
    }
}
