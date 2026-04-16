namespace BusesEscolares_NOSQL.API.Models
{
    public class DatabaseSettings
    {
        public string Database { get; set; } = null!;
        public string ColeccionRutas { get; set; } = null!;
        public string ColeccionBuses { get; set; } = null!;
        public string ColeccionViajes { get; set; } = null!;
        public string ColeccionZonas { get; set; } = null!;
        public string ColeccionConductores { get; set; } = null!;

        public DatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("DatabaseSettings");

            Database = configuracion.GetSection("Database").Value!;
            ColeccionBuses = configuracion.GetSection("BusesCollection").Value!;
            ColeccionZonas = configuracion.GetSection("ZonesCollection").Value!;
            ColeccionRutas = configuracion.GetSection("RoutesCollection").Value!;
            ColeccionConductores = configuracion.GetSection("DriversCollection").Value!;
            ColeccionViajes = configuracion.GetSection("TripsCollection").Value!;
        }
    }
}