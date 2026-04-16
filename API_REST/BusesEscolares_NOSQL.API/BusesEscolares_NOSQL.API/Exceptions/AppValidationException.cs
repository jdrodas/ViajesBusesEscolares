/*
AppValidationException:
Excepcion creada para enviar mensajes relacionados 
con la validación de datos en las capas de servicio
*/

namespace BusesEscolares_NOSQL.API.Exceptions
{
    public class AppValidationException(string message) : Exception(message)
    {
    }
}