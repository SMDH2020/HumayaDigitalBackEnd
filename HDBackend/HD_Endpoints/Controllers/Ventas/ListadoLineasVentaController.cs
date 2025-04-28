using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Ventas.Consultas;
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

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarLinea(string descripcion, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Linea_Venta datos = new AD_Agregar_Linea_Venta(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Linea(descripcion, usuario);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarLinea(int idlinea, string descripcion, int estatus, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Linea_Venta datos = new AD_Editar_Linea_Venta(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.EditarLinea(idlinea, descripcion, estatus, usuario);
            return Ok(result);
        }
    }
}
