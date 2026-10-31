using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.CodigoVendedoresEquip;
using Postventa.Consultas.PartesFamilia;
using Postventa.Consultas.PartesMultiplicador;
using Postventa.Modelos.CodigoVendedoresEquip;
using Postventa.Modelos.PartesFamilia;

namespace HD.Endpoints.Controllers.Postventa
{
    public class PartesFamiliaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PartesFamiliaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Partes_Familia mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Partes_Familia_Guardar datos = new AD_Partes_Familia_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Partes_Familia_Listado datos = new AD_Partes_Familia_Listado(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Catalogo()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Partes_Familia_Catalogo datos = new AD_Partes_Familia_Catalogo(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Catalogo();
            return Ok(result);
        }
    }
}
