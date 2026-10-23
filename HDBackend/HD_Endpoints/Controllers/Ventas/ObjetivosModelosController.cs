using HD.Security;
using HD_Ventas.Consultas.ObjetivosModelos;
using HD_Ventas.Modelos.ObjetivosModelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ObjetivosModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObjetivosModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int ejercicio, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Objetivos_Modelos_Listado datos = new AD_Objetivos_Modelos_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, sucursal);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Objetivos_Modelos_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Objetivos_Modelos_Guardar datos = new AD_Objetivos_Modelos_Guardar(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.Guardar(mdl, usuario);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }
    }
}
