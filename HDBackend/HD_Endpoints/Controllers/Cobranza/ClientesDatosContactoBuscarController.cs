using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ClientesDatosContactoBuscarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesDatosContactoBuscarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> MostrarDatosContacto(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Datos_Contacto_Cargar datos = new AD_Clientes_Datos_Contacto_Cargar(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);
        }
    }
}
