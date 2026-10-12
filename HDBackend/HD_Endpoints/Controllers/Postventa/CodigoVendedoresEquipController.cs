using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.CodigoVendedoresEquip;
using Postventa.Consultas.Dashboard;
using Postventa.Modelos.CodigoVendedoresEquip;

namespace HD.Endpoints.Controllers.Postventa
{
    public class CodigoVendedoresEquipController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CodigoVendedoresEquipController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Codigo_Vendedores_Equip_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Codigo_Vendedores_Equip_Guardar datos = new AD_Codigo_Vendedores_Equip_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Codigo_Vendedores_Equip_Listado datos = new AD_Codigo_Vendedores_Equip_Listado(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Codigos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Codigo_Vendedores_Equip_Codigos datos = new AD_Codigo_Vendedores_Equip_Codigos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado();
            return Ok(result);
        }

    }
}
