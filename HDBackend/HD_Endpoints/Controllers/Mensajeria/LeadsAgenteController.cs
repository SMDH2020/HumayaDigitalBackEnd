using HD.Security;
using HD_Mensajeria.Consultas;
using HD_Mensajeria.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Mensajeria
{
    public class LeadsAgenteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public LeadsAgenteController(IConfiguration configuration, ISesion sesion) 
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerLeads()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Leads_Agente datos = new AD_Leads_Agente(CadenaConexion);
            var result = await datos.obtenerLeads();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarLead(mdl_Guardar_Leads_Agente mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Leads_Agente datos = new AD_Leads_Agente(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarLead(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarLead(mdl_Actualiza_Lead mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Leads_Agente datos = new AD_Leads_Agente(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.ActualizarLead(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarLead(int idlead)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Leads_Agente datos = new AD_Leads_Agente(CadenaConexion);
            var result = await datos.EliminaLead(idlead);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CambiaEstatusLead(int idlead)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Leads_Agente datos = new AD_Leads_Agente(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.CambiaEstatusLead(idlead, usuario);
            return Ok(result);
        }
    }
}
