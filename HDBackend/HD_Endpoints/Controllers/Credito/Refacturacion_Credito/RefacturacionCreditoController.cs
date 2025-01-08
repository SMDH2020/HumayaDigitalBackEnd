using HD.Clientes.Consultas.PrestamoClientes;
using HD.Clientes.Modelos.PrestamoClientes;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD.Clientes.Consultas.Refacturacion_Credito;
using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis.JDF;

namespace HD.Endpoints.Controllers.Credito.Refacturacion_Credito
{
    public class RefacturacionCreditoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RefacturacionCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Refacturacion_Credito_Guardar datos = new AD_Refacturacion_Credito_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> SolicitudesListado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Aceptadas_Listado datos = new AD_Solicitudes_Aceptadas_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }
    }
}
