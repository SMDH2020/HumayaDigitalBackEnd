using HD.Security;
using HD_Reporteria.Ventas;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class SolicitudesCreditoFacturadasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudesCreditoFacturadasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> SolicitudesFacturadas(int ejercicio, int periodo, string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Facturadas datos = new AD_Solicitudes_Facturadas(CadenaConexion);
            var result = await datos.GetSolicitudes(ejercicio, periodo, linea);
            result.resumen.titulo = $"RESULTADO DE OPERACIONES {nombre_mes(periodo)} - {ejercicio}";
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> SolicitudesFacturadasDetalle(int ejercicio, int periodo,int idsucursal, string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Facturadas datos = new AD_Solicitudes_Facturadas(CadenaConexion);
            var result = await datos.GetSolicitudesDetalle(ejercicio, periodo,idsucursal, linea);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> SolicitudesFacturadasCardDetalle(int ejercicio, int periodo, int idsucursal, string linea,string card)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Facturadas datos = new AD_Solicitudes_Facturadas(CadenaConexion);
            var result = await datos.GetSolicitudesDetalle(ejercicio, periodo, idsucursal, linea,card);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(int ejercicio, int periodo, string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_Facturadas_Excel datos = new AD_Solicitudes_Facturadas_Excel(CadenaConexion);
            var result = await datos.Listado(ejercicio, periodo, linea);
            var docresult = await XLS_Solicitudes_Facturadas.CrearExcel(result, "TODO", periodo, ejercicio);
            return Ok(docresult);
        }

        string nombre_mes(int mes)
        {
            string nombre = "";
            switch(mes)
            {
                case 1: nombre = "ENERO"; break;
                case 2: nombre = "FEBRERO"; break;
                case 3: nombre = "MARZO"; break;
                case 4: nombre = "ABRIL"; break;
                case 5: nombre = "MAYO"; break;
                case 6: nombre = "JUNIO"; break;
                case 7: nombre = "JULIO"; break;
                case 8: nombre = "AGOSTO"; break;
                case 9: nombre = "SEPTIEMBRE"; break;
                case 10: nombre = "OCTUBRE"; break;
                case 11: nombre = "NOVIEMBRE"; break;
                case 12: nombre = "DICIEMBRE"; break;
            }
            return nombre;
        }
    }
}
