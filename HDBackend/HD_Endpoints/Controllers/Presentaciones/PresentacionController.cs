using HD.Generales.Consultas;
using HD.Generales.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Presentaciones
{
    public class PresentacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PresentacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Buscar(Guid id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Buscar(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado();
            return Ok(result);
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Presentaciones_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            mdl.usuario = Sesion.usuario();

            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Actualizar(mdl_Presentaciones_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            mdl.usuario = Sesion.usuario();

            var result = await datos.Actualizar(mdl);
            return Ok(result);
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarHtml(mdl_Presentaciones_Html mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Presentaciones datos = new AD_Presentaciones(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarHtml(mdl);
            return Ok(result);
        }
    }
}
