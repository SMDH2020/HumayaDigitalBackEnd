﻿using HD.Security;
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
        public async Task<ActionResult> ReporteConvenioPDF(mdlConvenio_Pago mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            try
            {
                RPT_Result documento = RPT_ConvenioPago.Generar(mdl,null);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
