using HD.Security;
using HD_Auditoria.Consultas.Carga_Archivos;
using HD_Auditoria.Consultas.Conteo_Piezas;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Auditoria.Modelos.Conteo_Piezas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Conteo_Piezas
{
    public class RealizarConteoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RealizarConteoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> RegistrarConteoOnline(mdl_Conteo_Piezas_Online mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            mdl.id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.RegistrarConteo(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Justificar(mdl_Justificar_Piezas_Conteo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            //mdl.id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.Justificar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetJustificacion(int id_inv_fisico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            var result = await datos.GetJustificacion(id_inv_fisico);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GuardaNuevaPosicion(mdl_Posicion_Extra mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            //mdl.id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.Agregar_Posicion_Extra(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> EliminaNuevaPosicion(mdl_Posicion_Extra mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            //mdl.id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.Eliminar_Posicion_Extra(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> AgregarNuevaPieza(mdl_Agregar_Nueva_Pieza mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conteo_Piezas_Online datos = new AD_Conteo_Piezas_Online(CadenaConexion);
            mdl.id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.AgregarNuevaPieza(mdl);
            return Ok(result);
        }
    }
}
