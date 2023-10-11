using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ComentariosFacturasController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ComentariosFacturasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Guardar(mdlCob_ComentariosFactura obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_ComentariosFacutras datos = new ADCob_ComentariosFacutras(CadenaConexion);
            var result = await datos.Guardar(obj);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente, int idfactura)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_ComentariosFacutras datos = new ADCob_ComentariosFacutras(CadenaConexion);
            var result = await datos.Listado(idcliente, idfactura);
            return Ok(result);

        }
    }
}
