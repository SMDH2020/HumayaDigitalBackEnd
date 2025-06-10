using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ListadoModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Modelos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Modelos datos = new AD_Listado_Modelos(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ModelosCotizacion()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Modelos datos = new AD_Listado_Modelos(CadenaConexion);
            var result = await datos.ListadoCompleto();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> LineasDropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Modelos datos = new AD_Listado_Modelos(CadenaConexion);
            var result = await datos.ListadoLineasDropdownlist();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerModelosID(int idmodelo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Modelos datos = new AD_Listado_Modelos(CadenaConexion);
            var result = await datos.ObtenerModeloID(idmodelo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCarrusel(int idmodelo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Lineas_Venta datos = new AD_Listado_Lineas_Venta(CadenaConexion);
            var result = await datos.Carrusel(idmodelo);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarModelo(mdl_Agregar_Modelo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Modelo datos = new AD_Agregar_Modelo(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarModelo(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarFotografia(int idmodelo, int numero)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Eliminar_Imagen_Modelo datos = new AD_Eliminar_Imagen_Modelo(CadenaConexion);
            var result = await datos.Eliminar(idmodelo, numero);
            return Ok(result);
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarModelo(mdl_Editar_Modelo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Modelo datos = new AD_Editar_Modelo(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.EditarModelo(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CambiarEstado(int idmodelo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Modelo datos = new AD_Editar_Modelo(CadenaConexion);
            var result = await datos.CambiarEstado(idmodelo);
            return Ok(result);
        }
    }
}
