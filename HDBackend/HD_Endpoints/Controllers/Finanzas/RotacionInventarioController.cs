using HD.Clientes.Consultas.Cultivos;
using HD.Security;
using HD_Finanzas.AccesoDatos.RotacionInventario;
using HD_Reporteria.Finanzas;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Reportes;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class RotacionInventarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RotacionInventarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Reporte(int ejercicio, int periodo, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Rotacion_Inventario_Reporte datos = new AD_Rotacion_Inventario_Reporte(CadenaConexion);
            var result = await datos.reporte(ejercicio, periodo, adr, sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporte(int ejercicio, int periodo, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Rotacion_Inventario_Reporte datos = new AD_Rotacion_Inventario_Reporte(CadenaConexion);
            var result = await datos.reporte(ejercicio, periodo, adr, sucursales);
            // 👉 Convertir periodo a mes en letra (español)
            string mes = CultureInfo
                            .GetCultureInfo("es-MX")
                            .DateTimeFormat
                            .GetMonthName(periodo)
                            .ToUpper();

            var titulo = $"ROTACION INVENTARIO {mes} - {ejercicio}";

            var docresult = await RPT_Rotacion_Inventario_XLS.GenerarExcel(result, titulo);
            return Ok(docresult);
        }
    }
}
