using HD.Clientes.Consultas.InteresMensual;
using HD.Clientes.Consultas.PedidoUnidades;
using HD.Clientes.Modelos;
using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class UsuarioImeiController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public UsuarioImeiController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlRelUsuarioImei mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuarioImei_Guardar datos = new AD_RelUsuarioImei_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idusuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuarioImei_BuscarID datos = new AD_RelUsuarioImei_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idusuario);
            return Ok(result);

        }
    }
}

