using HD.Clientes.Consultas.PedidoUnidades;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ConvenioPagoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ConvenioPagoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlConvenio_Pago mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADConvenio_Pago datos = new ADConvenio_Pago(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result=await datos.Guardar(mdl);
            try
            {
                RPT_Result documento = RPT_ConvenioPago.Generar(mdl, result);
                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteConvenioPDF(mdlConvenio_Pago mdl )
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];

            try
            {
                ADVencimientosSaldos datos = new ADVencimientosSaldos(CadenaConexion);
                IEnumerable<mdlVencidosOperacion> result;
                if (mdl.tipo_credito == "O")
                {
                    result = await datos.ObtenerOperacion(mdl.idcliente);
                }
                else
                {
                    result = (await datos.ObtenerRevolvente(mdl.idcliente)).OfType<mdlVencidosOperacion>();
                };

                
                RPT_Result documento = RPT_ConvenioPago.Generar(mdl,result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error de servidor: {ex.Message}");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADConvenioListado datos = new ADConvenioListado(CadenaConexion);
            var result = await datos.Get(idcliente);
            return Ok(result);

        }
    }
}
