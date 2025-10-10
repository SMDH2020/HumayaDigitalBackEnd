using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Inventario;
using Usados.Modelos.Inventario;

namespace HD.Endpoints.Controllers.Ventas
{
    public class PromocionesDisponiblesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PromocionesDisponiblesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PromocionesDisponibles(string estado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.ObtenerPromocionesDisponibles(estado);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerPromocionID(int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.ObtenerPromocionID(idpromocion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarPromocion (string descripcion, string inicio_vigencia, string vigencia, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.AgregarPromocion(descripcion, inicio_vigencia, vigencia, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarPromocion(int idpromocion,string descripcion, string inicio_vigencia, string vigencia, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.EditarPromocion(idpromocion, descripcion, inicio_vigencia, vigencia, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerModelosEsquema(int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.ObtenerModelosEsquema(idpromocion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarModelosdeEsquema(int idmodelo, int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.EliminarModelodeEsquema(idmodelo, idpromocion, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> RestaurarModelosdeEsquema(int idmodelo, int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.RestaurarModelodeEsquema(idmodelo, idpromocion);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarModelosEsquema(mdl_Agregar_Modelos_Esquema mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            var usuario = int.Parse(Sesion.usuario());
            AD_Promociones_Disponibles datos_documentos = new AD_Promociones_Disponibles(CadenaConexion);
            foreach (mdl_Modelos_Esquema data in mdl.modelosEsquema)
            {
                await datos_documentos.AgregarModelosEsquema(data.idmodelo, data.idpromocion, data.costo_refacciones, data.costo_servicios, data.precio_promocion, usuario);
            }

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ModelosenEsquema()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.ObtenerModelosEnEsquema();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerEsquemasporModelo(int idmodelo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.ObtenerEsquemasporModelo(idmodelo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEsquemasDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Promociones_Disponibles datos = new AD_Promociones_Disponibles(CadenaConexion);
            var result = await datos.GetEsquemasDDL();
            return Ok(result);
        }
    }
}
