using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.CodigoVendedoresEquip;
using Postventa.Consultas.PartesMultiplicador;
using Postventa.Modelos.CodigoVendedoresEquip;
using Postventa.Modelos.PartesMultiplicador;

namespace HD.Endpoints.Controllers.Postventa
{
    public class MultiplicadorPiezaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MultiplicadorPiezaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Partes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Piezas_Multiplicador_Guardar datos = new AD_Piezas_Multiplicador_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Partes_Multiplicador_Listado datos = new AD_Partes_Multiplicador_Listado(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> partes()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Partes_Multiplicador_Listado_Partes datos = new AD_Partes_Multiplicador_Listado_Partes(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ListadoPartes();
            return Ok(result);
        }
    }
}
