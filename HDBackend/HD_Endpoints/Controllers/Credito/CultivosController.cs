using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class CultivosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CultivosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlCultivos mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Cultivos_Guardar datos = new AD_Cultivos_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Cultivos_Listado datos = new AD_Cultivos_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idcultivo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Cultivos_BuscarID datos = new AD_Cultivos_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idcultivo);
            return Ok(result);

        } 

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Cultivos_DropDownList datos = new AD_Cultivos_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
