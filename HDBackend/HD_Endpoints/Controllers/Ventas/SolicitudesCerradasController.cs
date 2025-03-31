using HD.Clientes.Consultas.SolicitudCredito_Analisis;
using HD.Security;
using HD_Ventas.Consultas.SolicitudesCerradas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class SolicitudesCerradasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudesCerradasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetTablero()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Cerradas_Tablero datos = new AD_Solicitudes_Cerradas_Tablero(CadenaConexion);
            var result = await datos.obtenerTablero(Sesion.usuario());
            return Ok(result);

        }
    }
}
