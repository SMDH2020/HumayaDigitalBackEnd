using HD.Security;
using HD_Cobranza.Capturas.AgregarContacto;
using HD_Cobranza.Capturas.ComentariosClientesContacto;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.AgregarContacto;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ComentariosClientesContacto
{
    public class ComentariosClientesContactoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ComentariosClientesContactoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(int idcliente, string comentario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Comentarios_ClientesContacto datos = new AD_Guarda_Comentarios_ClientesContacto(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Comentario(idcliente, comentario, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Mostrar(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Comentarios_ClientesContacto datos = new AD_Carga_Comentarios_ClientesContacto(CadenaConexion);
            var result = await datos.Comentarios(idcliente);
            return Ok(result);
        }
    }
}
