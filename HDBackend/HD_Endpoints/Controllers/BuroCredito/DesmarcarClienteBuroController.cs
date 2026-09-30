using HD.Security;
using HD_Buro.Consultas;
using Microsoft.AspNetCore.Mvc;
namespace HD.Endpoints.Controllers.BuroCredito
{
    public class DesmarcarClienteBuroController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DesmarcarClienteBuroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> desmarcar(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Desmarcar_ClienteBuro datos = new AD_Desmarcar_ClienteBuro(CadenaConexion);
            var result = await datos.cliente(idcliente);
            return Ok(result);
        }
    }
}
