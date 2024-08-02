using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.PedidoUnidades;
using HD.Clientes.Consultas.PrestamoClientes;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.PrestamoClientes;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito.PrestamoClientes
{
    public class PrestamoClientesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PrestamoClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlPrestamo_Clientes_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Prestamo_Clientes_Guardar datos = new AD_Prestamo_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarRel(mdl);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Crear_Timeline(mdlPrestamoClientesView mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Prestamo_Clientes_Guardar datos = new AD_Prestamo_Clientes_Guardar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Crear_Timeline(mdl);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitudes_JDF_Listado datos = new AD_Solicitudes_JDF_Listado(CadenaConexion);
            var result = await datos.Get();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalle(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Prestamo_Clientes_ObtenerDetalle datos = new AD_Prestamo_Clientes_ObtenerDetalle(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
    }
}
