using HD.AccesoDatos;
using Newtonsoft.Json;
using System.Net;

namespace HD.Endpoints.Middleware
{
    public class ManejadorMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorMiddlewares> _logger;
        public ManejadorMiddlewares(RequestDelegate next, ILogger<ManejadorMiddlewares> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExceptionAsync(context, ex, _logger);
            }
        }

        private async Task ManejadorExceptionAsync(HttpContext context, Exception ex, ILogger<ManejadorMiddlewares> logger)
        {
            object? errores = null;
            switch (ex)
            {
                case Excepciones m:
                    logger.LogError(ex, "Manejador de Errores");
                    errores = m.errores;
                    context.Response.StatusCode = (int)m.statuscode;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error del servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message)
                        ? new { error = "ERROR DE EL SERVIDOR, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA " }
                        : new { error = e.Message };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errores != null)
            {
                var result = JsonConvert.SerializeObject(errores);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
