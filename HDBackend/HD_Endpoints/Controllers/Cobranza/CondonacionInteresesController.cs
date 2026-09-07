using HD.Security;
using HD_Cobranza.Capturas.CondonacionIntereses;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.CondonacionIntereses;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class CondonacionInteresesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CondonacionInteresesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Guarda_Usuarios_Autorizan_Condonacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Eliminar(mdl_Elimina_Usuario_Autoriza_Condonacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Eliminar(mdl);

            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> UsuarioCondonacionID(int idAutoriza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerUsuarioCondonacionID(idAutoriza);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoUsuariosCondonacion()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ListadoUsuarios();
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerLimiteUsuario()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerLimiteUsuario(usuario);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCondonacion(mdl_Guarda_Condonacion_Interes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCondonacionesCliente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerCondonacionesCliente(idcliente);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ValidarCondonacionesCliente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ValidarCondonacionesCliente(idcliente);

            return Ok(result);
        }
    }
}
