using HD.Security;
using HD_Buro.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaDetalleBuroController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaDetalleBuroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar_Vencido(int ejercicio, int periodo, string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Detalle_Buro_Vencido datosVencido = new AD_Carga_Detalle_Buro_Vencido(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datosVencido.detalle_vencido(ejercicio, periodo, idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar_porVencer(int ejercicio, int periodo, string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Detalle_Buro_PorVencer datosPorvencer = new AD_Carga_Detalle_Buro_PorVencer(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datosPorvencer.detalle_porvencer(ejercicio, periodo, idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar_Comentario(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_ClientesBuro_Comentarios datosComentario = new AD_Carga_ClientesBuro_Comentarios(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datosComentario.comentarios(idcliente);
            return Ok(result);
        }
    }
}
