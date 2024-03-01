using HD.Clientes.Consultas.Facturar_Equipo;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Clientes
{
    public class FacturarEquipoController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturarEquipoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Solicitudes()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturar_Equipo datos = new AD_Facturar_Equipo(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);

        }
    }
}
