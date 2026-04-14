using HD.Security;
using HD_Finanzas.AccesoDatos.RotacionCXC;
using HD_Finanzas.AccesoDatos.RotacionInventario;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class RotacionCXCController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RotacionCXCController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        //[HttpPost]
        //[Route("/api/[controller]/[action]")]
        //public async Task<ActionResult> GuardarGuiaCXC(int ejercicio)
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    GuiaCXCListado datos = new GuiaCXCListado(CadenaConexion);
        //    var result = await datos.Listado(ejercicio);
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoGuiaCXC(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            GuiaCXCListado datos = new GuiaCXCListado(CadenaConexion);
            var result = await datos.Listado(ejercicio);
            return Ok(result);
        }
    }
}
