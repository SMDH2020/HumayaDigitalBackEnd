using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ListadoLineasVentaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoLineasVentaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Lineas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Lineas_Venta datos = new AD_Listado_Lineas_Venta(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerLineaID(int idlinea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Lineas_Venta datos = new AD_Listado_Lineas_Venta(CadenaConexion);
            var result = await datos.ObtenerLineaID(idlinea);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarLinea(mdlListadoLineasVentas mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Linea_Venta datos = new AD_Agregar_Linea_Venta(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Linea(mdl);
            return Ok(result);
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarLinea(mdlListadoLineasVentas mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Linea_Venta datos = new AD_Editar_Linea_Venta(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.EditarLinea(mdl);
            return Ok(result);
        }
    }
}
