using HD.AccesoDatos;
using HD.Security;
using HD_Auditoria.Consultas;
using HD_Auditoria.Consultas.Carga_Archivos;
using HD_Auditoria.Modelos;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Usados;
using Ventas.Consultas;

namespace HD.Endpoints.Controllers.Auditoria.Carga_Archivos
{
    public class CargaInventarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CargaInventarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> CargarInventarioFisico(mdl_Cargar_Inventario_Fisico mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Inventario_Fisico datos = new AD_Carga_Inventario_Fisico(CadenaConexion);
            mdl.id_usuario = int.Parse(Sesion.usuario());
            var result = await datos.Carga_Inventario_Fisico(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> CargarInventarioAjustes(mdl_Cargar_Inventario_Ajustes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Inventario_Ajustes datos = new AD_Carga_Inventario_Ajustes(CadenaConexion);
            mdl.id_usuario = int.Parse(Sesion.usuario());
            var result = await datos.Carga_Inventario_Ajustes(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> AsignaPasillo(mdl_Asignacion_Pasillos mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Asignacion_Pasillos datos = new AD_Asignacion_Pasillos(CadenaConexion);
            var result = await datos.Asignacion_Pasillos(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoAuditoriasProg()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Auditorias_Programadas datos = new AD_Listado_Auditorias_Programadas(CadenaConexion);
            var result = await datos.Auditorias();
            return Ok(result);

        }
    }
}
