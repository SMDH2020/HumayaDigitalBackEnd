using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF
{
    public class ACJDF_Analisis_AsignarPromotorController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACJDF_Analisis_AsignarPromotorController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AsiganarPromotor(mdlJDFAnalisis_Asignar_Promotor_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            mdl.usuario = Sesion.usuario();
            ADJDF_Analisis_Asignar_promotor_comentario datos = new ADJDF_Analisis_Asignar_promotor_comentario(CadenaConexion);
            var result = await datos.Get(mdl);

            //ADJDF_Asignacion_Promotor_Notificacion notificacion = new ADJDF_Asignacion_Promotor_Notificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            return Ok(result);


        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_AsignarPromotor datos = new ADJDF_Analisis_AsignarPromotor(CadenaConexion);
            var result = await datos.Get(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
