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
    }
}
