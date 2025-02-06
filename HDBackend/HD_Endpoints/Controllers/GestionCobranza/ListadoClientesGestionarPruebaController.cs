using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ListadoClientesGestionarPruebaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoClientesGestionarPruebaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoClientes(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_Prueba datos = new AD_Listado_Clientes_Gestionar_Prueba(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, convenio, juridico);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoClientes2(string adr, string sucursal, string responsable, string linea, string cartera, string gestion, int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_2 datos = new AD_Listado_Clientes_Gestionar_2(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, gestion, ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_Prueba datos = new AD_Listado_Clientes_Gestionar_Prueba(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, convenio, juridico);
            var docresult = await XLSCob_Listado_Clientes_Gestionar_Prueba.GenerarExcel(result);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelCarteraCliente(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_Prueba datos = new AD_Listado_Clientes_Gestionar_Prueba(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, convenio, juridico);
            var docresult = await XLSCre_Listado_Cartera_Clientes.GenerarExcel(result);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_Prueba datos = new AD_Listado_Clientes_Gestionar_Prueba(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, convenio, juridico);

            try
            {
                RPT_Result documento = RPT_Listado_Clientes_Gestionar_Prueba.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFCarteraClientes(string adr, string sucursal, int responsable, string linea, string cartera, string convenio, string juridico)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar_Prueba datos = new AD_Listado_Clientes_Gestionar_Prueba(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable, linea, cartera, convenio, juridico);

            try
            {
                RPT_Result documento = RPT_Listado_Cartera_Clientes.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
