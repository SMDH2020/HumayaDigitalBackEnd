using HD.Security;
using Microsoft.AspNetCore.Mvc;
using ProductoAliado.Consultas.Inventario;
using ProductoAliado.Modelos.Inventario;

namespace HD.Endpoints.Controllers.ProductoAliado.Inventario
{
    public class ImagenesProductoAliadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ImagenesProductoAliadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarImagenes(mdl_Imagenes_Producto_Aliado mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Producto_Aliado_Guardar datos = new AD_Imagenes_Producto_Aliado_Guardar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarImagenes(int idinventario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Producto_Aliado_Guardar datos = new AD_Imagenes_Producto_Aliado_Guardar(CadenaConexion);
            var result = await datos.Buscar(idinventario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarImagenes(int id_imagen)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Producto_Aliado_Guardar datos = new AD_Imagenes_Producto_Aliado_Guardar(CadenaConexion);
            var result = await datos.Eliminar(id_imagen);
            return Ok(result);

        }
    }
}
