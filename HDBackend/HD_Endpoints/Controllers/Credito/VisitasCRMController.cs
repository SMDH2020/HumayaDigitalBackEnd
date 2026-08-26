using HD.Clientes.Consultas.CRM.Visitas;
using HD.Clientes.Modelos.CRM.Visitas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class VisitasCRMController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public VisitasCRMController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoVisitasProgramadas(int ejercicio, int periodo, string fechainicio, string fechafin, int vendedor, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            vendedor = int.Parse(Sesion.usuario());
            var result = await datos.ListadoVisitasProgramadas(ejercicio, periodo, fechainicio, fechafin, vendedor, adr, sucursal);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerVisitaID(int idvisita)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            var result = await datos.ObtenerVisitaID(idvisita);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ProgramarVisita(mdl_Programar_Visita_CRM mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.ProgramarVisita(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerTimeLine(int idvisita)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            var result = await datos.ObtenerTimeLine(idvisita);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarEstatusVisita(mdl_Guarda_Estatus_Visita_CRM mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            mdl.createuser = int.Parse(Sesion.usuario());
            await datos.GuardarEstatusVisita(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminaVisita(int id_visita)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Visitas_CRM datos = new AD_Visitas_CRM(CadenaConexion);
            await datos.EliminaVisita(id_visita);
            return Ok(new { mensaje = "datos eliminados con exito" });
        }
    }
}
