using HD.Clientes.Consultas.GiroEmpresarial;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class GiroEmpresarialController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GiroEmpresarialController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlGiro_Empresarial mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_GiroEmpresarial_Guardar datos = new AD_GiroEmpresarial_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_GiroEmpresarial_Listado datos = new AD_GiroEmpresarial_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idgiroempresarial)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_GiroEmpresarial_BuscarID datos = new AD_GiroEmpresarial_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idgiroempresarial);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_GiroEmpresarial_DropDownList datos = new AD_GiroEmpresarial_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}

