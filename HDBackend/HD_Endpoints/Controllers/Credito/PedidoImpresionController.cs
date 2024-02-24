using HD.Clientes.Consultas.PedidoImpresion;
using HD.Clientes.Consultas.SolicitudImpresion;
using HD.Security;
using HD_Reporteria.Solicitud_Credito;
using HD_Reporteria;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PedidoImpresionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidoImpresionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADPedido_Impresion_View datos = new ADPedido_Impresion_View(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADPedido_Impresion_View datos = new ADPedido_Impresion_View(CadenaConexion);
            var result = await datos.Get(folio);


            try
            {
                RPT_Result documento = result.condiciones.mhusajdf=="JDT" ? RPT_Pedido.Generar(result) : RPT_Pedido_JDF.Generar(result) ;

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
