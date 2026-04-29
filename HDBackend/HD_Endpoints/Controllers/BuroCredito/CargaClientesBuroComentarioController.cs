using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Buro_Credito;
using HD_Buro.Modelos;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaClientesBuroComentarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaClientesBuroComentarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
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
