using HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado;
using HD.Clientes.Consultas.PedidoImpresion;
using HD.Security;
using HD_Reporteria;
using HD_Reporteria.ImagentoPDF;
using HD_Reporteria.Solicitud_Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ConstructorImagentoPDF : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ConstructorImagentoPDF(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DescargarPDF(mdl_Covertor mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            RPT_Result documento = CreadorPDF.Generar(mdl);

            ADSolicitud_Credito_Documentacion_JDF_Guardar datos = new ADSolicitud_Credito_Documentacion_JDF_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.vigencia = DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd");
            var result = await datos.GuardarIMGtoPDF(mdl.folio, mdl.iddocumento, documento.documento, mdl.comentarios, documento.extension, mdl.vigencia, mdl.usuario);

            return Ok(documento);
        }
    }
}
