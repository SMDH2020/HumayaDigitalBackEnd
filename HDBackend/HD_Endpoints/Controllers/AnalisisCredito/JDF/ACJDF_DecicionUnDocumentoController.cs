using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF
{
    public class ACJDF_DecicionUnDocumentoController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACJDF_DecicionUnDocumentoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> GetUnDocumento(mdlJDFAnalisis_Un_Documento_Decicion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisisDecicion datos = new ADAnalisisDecicion(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GetUndocumento(mdl);
            return Ok(result);
        }
    }
}
