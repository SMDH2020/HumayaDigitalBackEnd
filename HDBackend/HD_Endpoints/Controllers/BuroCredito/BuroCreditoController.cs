using HD.Security;
using HD_Buro.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class BuroCreditoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public BuroCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> excel(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Buro_Credito_Reporte_Excel datos = new AD_Buro_Credito_Reporte_Excel(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            usuario = 8919;
            var result = await datos.reporte(ejercicio, periodo);
            return Ok(result);
        }
    }
}
