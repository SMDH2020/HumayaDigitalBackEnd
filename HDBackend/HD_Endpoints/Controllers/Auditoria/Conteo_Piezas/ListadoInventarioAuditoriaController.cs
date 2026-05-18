using HD.Security;
using HD_Auditoria.Consultas.Carga_Archivos;
using HD_Auditoria.Consultas.Conteo_Piezas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Conteo_Piezas
{
    public class ListadoInventarioAuditoriaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoInventarioAuditoriaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Inventario(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Inventario_Conteo_Piezas datos = new AD_Listado_Inventario_Conteo_Piezas(CadenaConexion);
            int id_auditor = int.Parse(Sesion.usuario());
            var result = await datos.Inventario(folio, id_auditor);
            return Ok(result);

        }
    }
}
