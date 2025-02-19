using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Inventario;
using Usados.Modelos.Inventario;
using Usados.Modelos.Usados;

namespace HD.Endpoints.Controllers.Usados.Inventario
{
    public class ImagenesMaquinariaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ImagenesMaquinariaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarImagenes(mdl_Imagenes_Maquinaria mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Maquinaria_Guardar datos = new AD_Imagenes_Maquinaria_Guardar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarImagenes(int idinventario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Maquinaria_Guardar datos = new AD_Imagenes_Maquinaria_Guardar(CadenaConexion);
            var result = await datos.Buscar(idinventario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarImagenes(int id_imagen)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Imagenes_Maquinaria_Guardar datos = new AD_Imagenes_Maquinaria_Guardar(CadenaConexion);
            var result = await datos.Eliminar(id_imagen);
            return Ok(result);

        }
    }
}
