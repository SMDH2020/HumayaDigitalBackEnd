using DocumentFormat.OpenXml.Math;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Modelos.CotizacionesVentas;
using Ventas.Reportes;
using HD.AccesoDatos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CotizacionVentasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionVentasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConsultarFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.ObtenerByFolio(usuario, folio);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarCotizacion(mdl_Agregar_Cotizacion_Nuevo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarCotizacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoCotizaciones(int usuario, string comparacion, string periodoinicio, string periodofin, string adr, string sucursal, int asesor, int cliente, int esquema, string fase)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario, comparacion, periodoinicio, periodofin, adr, sucursal, asesor, cliente, esquema, fase);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoCotizacionesMultiplataforma(int usuario, string comparacion, string periodoinicio, string periodofin, string adr, string sucursal, int asesor, int cliente, int esquema, string fase)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.ListadoMultiplataforma(usuario, comparacion, periodoinicio, periodofin, adr, sucursal, asesor, cliente, esquema, fase);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporte(int usuario, string comparacion, string periodoinicio, string periodofin, string adr, string sucursal, int asesor, int cliente, int esquema, string fase, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario, comparacion, periodoinicio, periodofin, adr, sucursal, asesor, cliente, esquema, fase);
            var docresult = await XLSVen_Listado_Cotizaciones.GenerarExcel(result, titulo);
            //var servicio = await Conexion_Servicio_Mensajeria.send("home", new { });
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarCotizacion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Eliminar_Cotizacion(folio, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DDLAsesores()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.DDLAsesores(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DDLClientes()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            //usuario = int.Parse(Sesion.usuario());
            var result = await datos.DDLClientes();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DDLEsquemas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            //usuario = int.Parse(Sesion.usuario());
            var result = await datos.DDLEsquemas();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Permisos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GetPermisos(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarComentario(string folio, string comentario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GuardarComentario(folio, comentario, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCultivo(string folio, string cultivo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GuardarCultivo(folio, cultivo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetComentarios(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerComentarios(folio);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CultivosListado(int adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cultivos_Listado datos = new AD_Cultivos_Listado(CadenaConexion);
            var result = await datos.Listado(adr);
            return Ok(result);
        }
    }
}
