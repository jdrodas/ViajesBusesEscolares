using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class RutaRepository(PgsqlDbContext unContexto) : IRutaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Ruta>> GetAllAsync(RutaParametrosConsulta parametrosConsulta)
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
                "SELECT DISTINCT r.id, r.nombre, r.distancia_kms distanciaKms, " +
                "r.zona_id zonaId, z.nombre zonaNombre " +
                "FROM core.rutas r JOIN core.zonas z on r.zona_id = z.id " +
                "ORDER BY nombre " +
                "LIMIT @elementosPagina " +
                "OFFSET @desfase";

            var resultadoRutas = await conexion
                .QueryAsync<Ruta>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoRutas];
        }

        public async Task<int> GetTotalAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.rutas";

            var totalRutas = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return totalRutas;
        }

        public async Task<Ruta> GetByIdAsync(Guid rutaId)
        {
            Ruta unaRuta = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@rutaId", rutaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT r.id, r.nombre, r.distancia_kms distanciaKms, " +
                "r.zona_id zonaId, z.nombre zonaNombre " +
                "FROM core.rutas r JOIN core.zonas z on r.zona_id = z.id " +
                "WHERE r.id = @rutaId";

            var resultado = await conexion
                .QueryAsync<Ruta>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaRuta = resultado.First();

            return unaRuta;
        }
    }
}
