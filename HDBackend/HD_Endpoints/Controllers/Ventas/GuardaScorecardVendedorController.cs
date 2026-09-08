using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class GuardaScorecardVendedorController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardaScorecardVendedorController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlGuarda_Scorecard_Vendedor mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Scorecard_Vendedor datos = new AD_Guarda_Scorecard_Vendedor(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        private ActionResult Ok(IEnumerable<mdlGuarda_Scorecard_Vendedor> result)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarReal(mdlGuarda_Scorecard_Real_Vendedor mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Scorecard_Real_Vendedor datos = new AD_Guarda_Scorecard_Real_Vendedor(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
