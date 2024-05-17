using HD.Clientes.Consultas.Pagares;
using HD.Clientes.Consultas.PedidoImpresion;
using HD.Security;
using HD_Cobranza.Capturas;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using HD_Reporteria.Pagares;
using HD_Reporteria.Solicitud_Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PagaresController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PagaresController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PagareDosAmortizacionesSuscripcion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Pagare_Dos_Amortizaciones_Suscripcion datos = new AD_Pagare_Dos_Amortizaciones_Suscripcion(CadenaConexion);
            var result = await datos.Get(folio);
            List<RPT_Result> documento = new List<RPT_Result>();

            // Primer conjunto de condiciones
            if (result.financiamientocerodias.Count() > 1)
            {
                try
                {
                    RPT_Result reporte = RPT_Pagare_Dos_Amortizaciones_Vencimiento.Generar(result);
                    documento.Add(reporte);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor");
                }
            }
            else if (result.financiamientocerodias.Count() == 1)
            {
                try
                {
                    RPT_Result reporte = RPT_Pagare_Vencimiento.Generar(result);
                    documento.Add(reporte);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor");
                }
            }

            // Segundo conjunto de condiciones
            if (result.financiamientomasdias.Count() > 1)
            {
                try
                {
                    RPT_Result reporte = RPT_Pagare_Dos_Amortizaciones_Suscripcion.Generar(result);
                    documento.Add(reporte);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor");
                }
            }
            else if (result.financiamientomasdias.Count() == 1)
            {
                try
                {
                    RPT_Result reporte = RPT_Pagare_Suscripcion.Generar(result);
                    documento.Add(reporte);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor");
                }
            }

            return Ok(documento);
        }



        //[HttpGet]
        //[Route("/api/[controller]/[action]")]
        //public async Task<ActionResult> PagareDosAmortizacionesVencimiento(string folio)
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    AD_Pagare_Dos_Amortizaciones_Suscripcion datos = new AD_Pagare_Dos_Amortizaciones_Suscripcion(CadenaConexion);
        //    var result = await datos.Get(folio);

        //    try
        //    {
        //        RPT_Result documento = RPT_Pagare_Dos_Amortizaciones_Vencimiento.Generar(result);

        //        return Ok(documento);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error de servidor");

        //    }
        //}

        //[HttpGet]
        //[Route("/api/[controller]/[action]")]
        //public async Task<ActionResult> PagareSuscripcion(string folio)
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    AD_Pagare_Dos_Amortizaciones_Suscripcion datos = new AD_Pagare_Dos_Amortizaciones_Suscripcion(CadenaConexion);
        //    var result = await datos.Get(folio);

        //    try
        //    {
        //        RPT_Result documento = RPT_Pagare_Suscripcion.Generar(result);

        //        return Ok(documento);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error de servidor");

        //    }
        //}

        //[HttpGet]
        //[Route("/api/[controller]/[action]")]
        //public async Task<ActionResult> PagareVencimiento(string folio)
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    AD_Pagare_Dos_Amortizaciones_Suscripcion datos = new AD_Pagare_Dos_Amortizaciones_Suscripcion(CadenaConexion);
        //    var result = await datos.Get(folio);

        //    try
        //    {
        //        RPT_Result documento = RPT_Pagare_Vencimiento.Generar(result);

        //        return Ok(documento);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error de servidor");

        //    }
        //}
    }
}
