using HD.Clientes.Consultas.Facturar_Equipo;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Clientes
{
    public class FacturarEquipoController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturarEquipoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Solicitudes()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturar_Equipo datos = new AD_Facturar_Equipo(CadenaConexion);
            int usuario = 8919;
                //int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> informacion_pedido(string folio)
        {
            if(folio ==null || folio .Length != 13)
            {
                return BadRequest("la información proporcionada no es correcta");
            }
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturar_Equipo_Estado datos = new AD_Facturar_Equipo_Estado(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.informacion(folio, usuario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> informacion_pedido_unidad(string folio,int registro)
        {
            if (folio == null || folio.Length != 13)
            {
                return BadRequest("la información proporcionada no es correcta");
            }
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturar_Equipo_Estado datos = new AD_Facturar_Equipo_Estado(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.informacion(folio,registro, usuario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarRegistro(string folio,int orden,int registro)
        {
            if (folio == null || folio.Length != 13)
            {
                return BadRequest("la información proporcionada no es correcta");
            }
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturar_Equipo_Estado datos = new AD_Facturar_Equipo_Estado(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
             await datos.EliminarRegistro(folio,registro,orden);
            return Ok(new {mensaje="Datos guardados con exito"});

        }
    }
}
