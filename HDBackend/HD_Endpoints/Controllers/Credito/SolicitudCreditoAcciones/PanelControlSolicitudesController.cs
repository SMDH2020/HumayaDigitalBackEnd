using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Consultas.SolicitudCreditoAcciones;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Solicitud_Credito_Acciones;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito.SolicitudCreditoAcciones
{
    public class PanelControlSolicitudesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PanelControlSolicitudesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> GuardarAccion(mdlSolicitud_Credito_Acciones mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitud_Credito_Acciones datos = new AD_Solicitud_Credito_Acciones(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }
    }
}
