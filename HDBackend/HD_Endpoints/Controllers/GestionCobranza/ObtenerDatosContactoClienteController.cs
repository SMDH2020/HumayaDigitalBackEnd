using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ObtenerDatosContactoClienteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObtenerDatosContactoClienteController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DatosContacto(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Datos_Contacto_Cliente datos = new AD_Obtener_Datos_Contacto_Cliente(CadenaConexion);
            var result = await datos.Datos(idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DatosMedio(int idmedio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Datos_Contacto_Medio datos = new AD_Obtener_Datos_Contacto_Medio(CadenaConexion);
            var result = await datos.Datos(idmedio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarDatos(int idmedio, string medio_contacto, string medio, string comentarios, int usuario, string responsable_pago)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Datos_Contacto_Cliente datos = new AD_Editar_Datos_Contacto_Cliente(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.NuevosDatos(idmedio, medio_contacto, medio, comentarios, usuario, responsable_pago);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DatosEliminar(int idmedio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Eliminar_Datos_Contacto_Cliente datos = new AD_Eliminar_Datos_Contacto_Cliente(CadenaConexion);
            var result = await datos.idmedio(idmedio);
            return Ok(result);
        }
    }
}
