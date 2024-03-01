 using HD.Clientes.Consultas.tasa_interes;
using HD.Security;
using HD_Cobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Clientes
{
    public class TasasJDFController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TasasJDFController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_tasas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADTasa_Interes datos = new ADTasa_Interes(CadenaConexion);
            var result = await datos.Buscartasas();
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_tasas_valores(int idtasa)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADTasa_Interes datos = new ADTasa_Interes(CadenaConexion);
            var result = await datos.Buscar_Tasas_valores(idtasa);
            return Ok(result);

        }

    }
}
