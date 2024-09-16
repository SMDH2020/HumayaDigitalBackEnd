using HD.Clientes.Consultas.Facturar_Equipo;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Clientes
{
    public class FacturarEquipoCreditoOtorgadoController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturarEquipoCreditoOtorgadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Solicitudes()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_FacturarEquipo_CreditoOtorgado datos = new AD_FacturarEquipo_CreditoOtorgado(CadenaConexion);
            int usuario = 8919; 
                //int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);

        }
    }
}
