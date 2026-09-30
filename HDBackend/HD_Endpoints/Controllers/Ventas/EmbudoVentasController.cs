using DocumentFormat.OpenXml.Math;
using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using HD_Ventas.Modelos.EmbudoVentas;
using HD_Ventas.Reportes;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Reportes;

namespace HD.Endpoints.Controllers.Ventas
{
    public class EmbudoVentasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public EmbudoVentasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerEmbudoVentas(string fecha_inicio, string fecha_fin, int esquema, string fase, int linea, string cultivo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Embudo_Ventas datos = new AD_Embudo_Ventas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerEmbudo(fecha_inicio, fecha_fin, esquema, fase, usuario, linea, cultivo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetLineaDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Embudo_Ventas datos = new AD_Embudo_Ventas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.DDLLineas();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporte(mdl_Embudo_Ventas_Excel post)
        {
            var data = post.Datos;
            var lineas = post.Lineas;
            var sucursales = post.Sucursales;
            var fases = post.Fases;
            var verPor = post.VerPor;
            var titulo = post.Titulo;
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            var docresult = await XLSVen_Embudo_Ventas.GenerarExcel(data, lineas, sucursales, fases, titulo, verPor);
            return Ok(docresult);
        }
    }
}
