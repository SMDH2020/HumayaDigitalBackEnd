using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Consultas.ClientesNoRegistrados;
using HD.Clientes.Modelos;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesNoRegistradosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesNoRegistradosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string ADR, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_No_Registrados datos = new AD_Clientes_No_Registrados(CadenaConexion);
            var result = await datos.Clientes(ADR, sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_No_Registrados datos = new AD_Clientes_No_Registrados(CadenaConexion);
            var result = await datos.Clientes(adr, sucursal);
            var docresult = await XLSCre_Listado_Clientes_NoRegistrados.GenerarExcel(result);
            return Ok(docresult);
        }
    }
}
