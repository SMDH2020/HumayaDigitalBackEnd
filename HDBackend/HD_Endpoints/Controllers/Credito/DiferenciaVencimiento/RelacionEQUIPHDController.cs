using HD.Clientes.Consultas.Credito;
using HD.Clientes.Consultas.Especiales;
using HD.Clientes.Modelos.Credito;
using HD.Clientes.Modelos.Especiales;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.DiferenciaVencimiento
{
    public class RelacionEQUIPHDController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RelacionEQUIPHDController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Relacion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Relacion_EQUIP_HD datos = new AD_Relacion_EQUIP_HD(CadenaConexion);
            var result = await datos.relacion(folio);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Actualizar(mdl_Actualizar_Relacion_EQUIP_HD mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Relacion_EQUIP_HD datos = new AD_Relacion_EQUIP_HD(CadenaConexion);
            var result = await datos.actualizar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }
    }
}
