using HD.Clientes.Consultas.Especiales;
using HD.Clientes.Modelos.Especiales;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class FacturasNoContemplarController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturasNoContemplarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlFacturasnocontemplar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturasNoContemplar datos = new ADFacturasNoContemplar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturasNoContemplar datos = new ADFacturasNoContemplar(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Buscar(int documento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturasNoContemplar datos = new ADFacturasNoContemplar(CadenaConexion);
            var result = await datos.Buscar(documento);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> InfoFactura(int documento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturasNoContemplar datos = new ADFacturasNoContemplar(CadenaConexion);
            var result = await datos.InfoFactura(documento);
            return Ok(result);

        }
    }
}
