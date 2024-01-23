using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF
{
    public class ACJDF_Cargar_FacturaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACJDF_Cargar_FacturaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            var result = await datos.Obtener(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
