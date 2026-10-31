using HD.Notifications.Consultas;
using HD.Notifications.Modelos;
using HD.Notifications;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD.Clientes.Consultas.RelacionDocumentosSC;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Modelos.CotizacionesVentas;
using HD.Clientes.Modelos.RelacionDocumentosSC;

namespace HD.Endpoints.Controllers.Credito.RelacionDocumentacionSC
{
    public class RelacionDocumentosSCController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RelacionDocumentosSCController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_RelacionDocumentosSC_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelacionDocumentosSC_Guardar datos = new AD_RelacionDocumentosSC_Guardar(CadenaConexion);
            var usuario = Sesion.usuario();
            await datos.Guardar(mdl, usuario);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelacionDocumentosSC_Listado datos = new AD_RelacionDocumentosSC_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> delete(int idmhusa, int idJDF)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelacionDocumentosSC_Delete datos = new AD_RelacionDocumentosSC_Delete(CadenaConexion);
            var usuario = Sesion.usuario();
            await datos.borrar(idmhusa, idJDF);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }
    }
}
