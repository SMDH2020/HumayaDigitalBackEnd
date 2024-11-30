using HD.Clientes.Consultas.Clientes_Juridico;
using HD.Clientes.Consultas.Especiales;
using HD.Clientes.Modelos.Clientes_Juridico;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.ClientesJuridico
{
    public class DetalleClientesJuridicoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DetalleClientesJuridicoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EstadoClientesDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Clientes_Juridico datos = new AD_Detalle_Clientes_Juridico(CadenaConexion);
            var result = await datos.EstadosCliente();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EstadoDemandaDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Clientes_Juridico datos = new AD_Detalle_Clientes_Juridico(CadenaConexion);
            var result = await datos.EstadosDemanda();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> InfoCliente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Clientes_Juridico datos = new AD_Detalle_Clientes_Juridico(CadenaConexion);
            var result = await datos.DetalleCliente(idcliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Guardar_Gestion_Judicial mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Clientes_Juridico datos = new AD_Detalle_Clientes_Juridico(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });
        }
    }
}
