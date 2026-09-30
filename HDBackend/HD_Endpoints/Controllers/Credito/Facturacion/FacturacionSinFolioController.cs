using HD.Clientes.Consultas.Facturacion;
using HD.Clientes.Modelos.Facturacion;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.Facturacion
{
    public class FacturacionSinFolioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturacionSinFolioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlFAC_FacturasByFolio data)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturacion_Guardar datos = new AD_Facturacion_Guardar(CadenaConexion);
            data.usuario = Sesion.usuario();
            var result = await datos.Guardar(data);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FacturasSinFolio(string filtro, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_FacturacionSinFolio datos = new AD_FacturacionSinFolio(CadenaConexion);
            var result = await datos.ObtenerFacturasSinFolio(filtro, sucursal);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GETBYID(string id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_FacturacionRegistrados datos = new AD_FacturacionRegistrados(CadenaConexion);
            var facturas = await datos.ObtenerFacturas(id);

            AD_FacturacionClientes cliente = new AD_FacturacionClientes(CadenaConexion);
            var clientes = await cliente.ObtenerClientes();

            AD_FacturacionVendedores vendedor = new AD_FacturacionVendedores(CadenaConexion);
            var vendedores = await vendedor.ObtenerVendedores();


            return Ok(new
            {
                clientes = clientes,
                factura = facturas,
                vendedor = vendedores
            });
        }
        [HttpGet("DiasFinanciamiento")]
        public async Task<ActionResult> DiasFinanciamiento(DateTime suscripcion, string vencimiento)
        {
            DateTime fechasuscripcion = DateTime.Parse(suscripcion.ToString("g", CultureInfo.CreateSpecificCulture("es-ES")));
            DateTime fechavencimiento = DateTime.Parse(vencimiento);
            var result = (fechavencimiento - fechasuscripcion).Days;
            return Ok(result);
        }
    }
}
