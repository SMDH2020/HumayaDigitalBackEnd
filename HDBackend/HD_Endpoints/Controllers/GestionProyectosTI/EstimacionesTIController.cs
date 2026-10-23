using HD.Security;
using HD_GestionProyectosTI.Consultas.Estimaciones;
using HD_GestionProyectosTI.Consultas.Roles;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class EstimacionesTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public EstimacionesTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        private async Task<bool> EsJefaturaTIAsync(int idusuario)
        {
            var roles = await new AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => r.Codigo == "JEFATURA_TI");
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Crear(mdl_TI_EstimacionCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            mdl.RegistradaPorUsuarioId = idusuario;

            var (estimacionId, numeroVersion) = await new AD_TI_Estimaciones_Crear(CadenaConexion).Crear(mdl);
            return Ok(new { estimacionId, numeroVersion, mensaje = "Estimacion registrada correctamente" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(long id)
        {
            var estimacion = await new AD_TI_Estimaciones_Obtener(CadenaConexion).Obtener(id);
            if (estimacion == null)
                return NotFound(new { mensaje = "Estimacion no encontrada" });

            return Ok(estimacion);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Validar(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Estimaciones_Validar(CadenaConexion).Validar(id, idusuario);
            return Ok(new { mensaje = "Estimacion validada" });
        }
    }
}
