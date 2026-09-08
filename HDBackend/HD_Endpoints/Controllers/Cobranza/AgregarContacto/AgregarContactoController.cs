using HD.Security;
using HD_Cobranza.Capturas.AgregarContacto;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.AgregarContacto;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.Cobranza.AgregarContacto
{
    public class AgregarContactoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AgregarContactoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlAgregarContacto mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Contacto_Guardar datos = new AD_Agregar_Contacto_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Contacto_Listado datos = new AD_Agregar_Contacto_Listado(CadenaConexion);
            var result = await datos.Get(idcliente);
            return Ok(result);

        }

       
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idmedio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Contacto_BuscarID datos = new AD_Agregar_Contacto_BuscarID(CadenaConexion);
            var result = await datos.Obtener(idmedio);
            return Ok(result);

        }

    }
}
