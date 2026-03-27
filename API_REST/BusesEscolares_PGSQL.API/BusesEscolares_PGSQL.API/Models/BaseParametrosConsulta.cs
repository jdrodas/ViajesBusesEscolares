namespace BusesEscolares_PGSQL.API.Models
{
    public class BaseParametrosConsulta
    {
        //Limite absoluto de elementos por página
        private const int MaximoElementosPorPagina = 50;

        private int _elementosPorPagina = 10;

        //Propiedades bases asignables por el usuario
        public int Pagina { get; set; } = 1;

        public int ElementosPorPagina
        {
            get
            {
                return _elementosPorPagina;
            }
            set
            {
                if (value > MaximoElementosPorPagina)
                    _elementosPorPagina = MaximoElementosPorPagina;
                else
                    _elementosPorPagina = value;
            }
        }
    }
}
