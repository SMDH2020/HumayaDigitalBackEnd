using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ListadoGestionesRealizadasComentarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoGestionesRealizadasComentarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoParametros(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, int objecion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Gestiones_Realizadas_Comentario datos = new AD_Listado_Gestiones_Realizadas_Comentario(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable, objecion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Objeciones()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Gestiones_Realizadas_Comentario datos = new AD_Listado_Gestiones_Realizadas_Comentario(CadenaConexion);
            var result = await datos.Objeciones();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, string? titulo, int objecion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Gestiones_Realizadas_Comentario datos = new AD_Listado_Gestiones_Realizadas_Comentario(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable, objecion);
            var docresult = await XLSCob_Listado_Gestiones_Realizadas.GenerarExcel(result, titulo);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, string? titulo, int objecion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Gestiones_Realizadas_Comentario datos = new AD_Listado_Gestiones_Realizadas_Comentario(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable, objecion);

            try
            {
                RPT_Result documento = RPT_Listado_Gestiones_Realizadas.GenerarPDF(result, titulo);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
