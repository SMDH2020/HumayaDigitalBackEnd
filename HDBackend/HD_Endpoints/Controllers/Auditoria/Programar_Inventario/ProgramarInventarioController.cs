using HD.Security;
using HD_Auditoria.Consultas.Carga_Archivos;
using HD_Auditoria.Consultas.Programar_Inventario;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Auditoria.Modelos.Programar_Inventario;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Programar_Inventario
{
    public class ProgramarInventarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ProgramarInventarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Listado datos = new AD_Programar_Inventario_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Listado(usuario);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Guardar(mdl_Programar_Inventario mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Guardar datos = new AD_Programar_Inventario_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.ProgramarInventario(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> EliminarProgramacion(mdl_ExtenderFecha mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Eliminar datos = new AD_Programar_Inventario_Eliminar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.EliminarRegistro(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> ListadoAuditores(string? folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Auditor_Listado datos = new AD_Programar_Inventario_Auditor_Listado(CadenaConexion);
            var result = await datos.buscarFolio(folio);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GuardarReasignacion(mdl_Auditores_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Reasignacion_Guardar datos = new AD_Programar_Inventario_Reasignacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.ProgramarInventario(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> ExtenderFecha(mdl_ExtenderFecha mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Programar_Inventario_Extender_Fecha datos = new AD_Programar_Inventario_Extender_Fecha(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.ExtenderFecha(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GuardarEncargados(mdl_Encargados mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Encargados_Sucursal_Guardar datos = new AD_Encargados_Sucursal_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarEncargados(mdl);
            return Ok(result);
        }

    }
}
