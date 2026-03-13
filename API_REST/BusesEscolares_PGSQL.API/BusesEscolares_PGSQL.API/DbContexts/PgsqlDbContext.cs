using Npgsql;
using System.Data;

namespace BusesEscolares_PGSQL.API.DbContexts
{
    public class PgsqlDbContext(IConfiguration unaConfiguracion)
    {
        private readonly string cadenaConexion = unaConfiguracion.GetConnectionString("BusesEscolaresPL")!;
        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(cadenaConexion);
        }
    }
}