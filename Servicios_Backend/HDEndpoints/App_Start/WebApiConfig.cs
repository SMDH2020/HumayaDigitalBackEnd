using System.Web.Http;

namespace HDEndpoints
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Configuración de CORS
            // Permitir todas las solicitudes desde cualquier origen.
            //var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();
            // Configuración y servicios de API web
         
            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Configurar formateadores
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
