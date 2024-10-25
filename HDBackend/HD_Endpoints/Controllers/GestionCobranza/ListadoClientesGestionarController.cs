using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ListadoClientesGestionarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoClientesGestionarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoClientes(string adr, string sucursal, int responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar datos = new AD_Listado_Clientes_Gestionar(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(string adr, string sucursal, int responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar datos = new AD_Listado_Clientes_Gestionar(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable);
            var docresult = await XLSCob_Listado_Clientes_Gestionar.GenerarExcel(result);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(string adr, string sucursal, int responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Gestionar datos = new AD_Listado_Clientes_Gestionar(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal, responsable);

            try
            {
                RPT_Result documento = RPT_Listado_Clientes_Gestionar.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
