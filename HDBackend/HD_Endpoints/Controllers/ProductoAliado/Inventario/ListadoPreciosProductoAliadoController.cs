using HD.Security;
using HD_Reporteria;
using HD_Reporteria.ProductoAliado;
using Microsoft.AspNetCore.Mvc;
using ProductoAliado.Consultas.Inventario;
using ProductoAliado.Modelos.Inventario;

namespace HD.Endpoints.Controllers.ProductoAliado.Inventario
{
    public class ListadoPreciosProductoAliadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoPreciosProductoAliadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarPrecio(mdl_Listado_Precio_Producto_Aliado mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Producto_Aliado_Guardar datos = new AD_Listado_Precio_Producto_Aliado_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.ActualizarPrecio(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarTodosPrecio(mdl_datosActualizadosProductoAliado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            var usuario = Sesion.usuario();
            AD_Listado_Precio_Producto_Aliado_Guardar datos_documentos = new AD_Listado_Precio_Producto_Aliado_Guardar(CadenaConexion);
            foreach (mdl_Listado_Precio_Producto_Aliado data in mdl.datosActualizados)
            {
                await datos_documentos.ActualizarTodosPrecio(data.idinventario, data.utilidad, data.margen, data.precio_lista, data.usuario);
            }

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string Modelo, int ejercicio, string HP, string Sucursal, string Promocion, string Estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.Listado(Modelo, ejercicio, HP, Sucursal, Promocion, Estatus);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoFiltro()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(IEnumerable<mdl_Inventario_Producto_Aliado> mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();

            try
            {
                RPT_Result documento = RPT_Listado_Precios.GenerarPDF(mdl);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirListadoPDF(IEnumerable<mdl_Inventario_Producto_Aliado> mdl)
        {
            // Concatenar todos los idinventario en una cadena separada por comas
            //string idinventario = string.Join(",", mdl.datosActualizados.Select(r => r.idinventario.ToString()));

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();

            try
            {
                RPT_Result documento = RPT_Listado_Precios_Corto.GenerarPDF(mdl);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarPromocion(mdl_promocion_Producto_Aliado mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Producto_Aliado_Guardar datos = new AD_Listado_Precio_Producto_Aliado_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.GuardarPromocion(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerPromocion(int idinventario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Obtener_Promocion datos = new AD_Listado_Precio_Obtener_Promocion(CadenaConexion);
            var result = await datos.BuscarID(idinventario);
            return Ok(result);

        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BorrarPromocion(int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Promocion_Borrar datos = new AD_Listado_Precio_Promocion_Borrar(CadenaConexion);
            var result = await datos.Borrar(idpromocion);
            return Ok(result);

        }
    }
}
