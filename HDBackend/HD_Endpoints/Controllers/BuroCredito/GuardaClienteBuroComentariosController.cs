using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Buro.Modelos;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class GuardaClienteBuroComentariosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardaClienteBuroComentariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlGuarda_ClientesBuro_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_ClientesBuro_Comentarios datos = new AD_Guarda_ClientesBuro_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
