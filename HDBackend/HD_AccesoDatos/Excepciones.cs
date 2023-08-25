using System.Net;

namespace HD.AccesoDatos
{
    public class Excepciones : Exception
    {
        public HttpStatusCode statuscode { get; }

        public object? errores { get; }

        public Excepciones(HttpStatusCode _statuscode, object? _errores=null)
        {
            statuscode = _statuscode;
            errores = _errores;
        }
    }
}
