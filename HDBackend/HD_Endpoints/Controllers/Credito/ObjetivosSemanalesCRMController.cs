using HD.Clientes.Consultas.CRM.ObjetivosSemanales;
using HD.Clientes.Modelos.CRM.ObjetivosSemanales;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ObjetivosSemanalesCRMController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObjetivosSemanalesCRMController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_ObjetivosSemanales_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ObjetivosSemanales_Guardar datos = new AD_ObjetivosSemanales_Guardar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ObjetivosSemanales_Listado datos = new AD_ObjetivosSemanales_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarSemanas(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ObjetivosSemanales_GenerarSemanas datos = new AD_ObjetivosSemanales_GenerarSemanas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.GenerarSemanas(ejercicio, usuario);
            return Ok(new { mensaje = "Registradas con exito" });
        }
    }
}
