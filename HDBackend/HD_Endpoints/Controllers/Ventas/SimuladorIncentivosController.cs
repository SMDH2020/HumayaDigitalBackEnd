using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class SimuladorIncentivosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SimuladorIncentivosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Roles()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Simulador_Incentivos datos = new AD_Simulador_Incentivos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            //usuario = 5630;
            var result = await datos.Obtener_Roles_Usuario(usuario);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_documento(string idrol)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Simulador_Incentivos datos = new AD_Simulador_Incentivos(CadenaConexion);

            var result = await datos.Obtener_Documento(idrol);
            return Ok(result);
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar_documento(mdlSimulador_documento_guardar obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Simulador_Incentivos datos = new AD_Simulador_Incentivos(CadenaConexion);
            obj.usuario = Sesion.usuario();
            //usuario = 5630;
            var result = await datos.Guardar_Documento(obj);
            return Ok(result);
        }
    }
}
