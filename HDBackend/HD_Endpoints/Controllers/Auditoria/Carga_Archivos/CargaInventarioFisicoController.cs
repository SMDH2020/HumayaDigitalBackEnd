using HD.AccesoDatos;
using HD.Security;
using HD_Auditoria.Consultas;
using HD_Auditoria.Consultas.Carga_Archivos;
using HD_Auditoria.Modelos;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Usados;

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
            var result = await datos.Carga_Inventario_Fisico(mdl);
            return Ok(result);
        }
    }
}
