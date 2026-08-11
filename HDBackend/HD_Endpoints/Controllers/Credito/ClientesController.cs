using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Consultas.CRM;
using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.CRM;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Fisica(mdlClientes_Datos_Persona_Fisica mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Fisica(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Moral(mdlClientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Moral(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Moral_Vendedor(mdlClientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Moral(mdl);
            return Ok(mdl.idcliente);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Moral_Registro_Vendedor(mdlClientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Vendedor_Guardar datos = new AD_Clientes_Vendedor_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idvendedor = mdl.idvendedor == "0" ? Sesion.usuario() : mdl.idvendedor;
            var result = await datos.Guardar_Persona_Moral(mdl); 
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Listado datos = new AD_Clientes_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_BuscarID datos = new AD_Clientes_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(int.Parse(idcliente));
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string usuario = Sesion.usuario();
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_DropDownList datos = new AD_Clientes_DropDownList(CadenaConexion);
            var result = await datos.DropDownList(usuario);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarRFCOrRazonSocial(string value)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_BuscarRFCOrRazonSocial datos = new AD_Clientes_BuscarRFCOrRazonSocial(CadenaConexion);
            var result = await datos.Listado(value);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetListadoClientesCRM()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Datos_CRM datos = new AD_Datos_CRM(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetInfoClienteID(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Datos_CRM datos = new AD_Datos_CRM(CadenaConexion);
            var result = await datos.Obtener_Info_Cliente(idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetLocalidadesCRM(string codigo_postal = null, int? idmunicipio = null)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Datos_CRM datos = new AD_Datos_CRM(CadenaConexion);
            var result = await datos.Listado_localidades(codigo_postal, idmunicipio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Coincidencia(string cliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_BuscarCoincidencias datos = new AD_Clientes_BuscarCoincidencias(CadenaConexion);
            var result = await datos.Get(cliente);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarRel(mdl_Rel_Cliente_Vendedor mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.GuardarRel(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarClasificacion(mdl_Cliente_Clasificacion mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Clasificacion_Guardar datos = new AD_Clientes_Clasificacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarClasificacion(mdl);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarDatosFacturacion(mdl_Datos_Facturacion mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Datos_Facturacion_Guardar datos = new AD_Clientes_Datos_Facturacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarDatosFacturacion(mdl);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GuardarDatosClasificacion(mdl_Guarda_Clasificacion_Cliente_CRM mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Datos_CRM datos = new AD_Datos_CRM(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.GuardarClasificacion(mdl);
            return Ok(result);
        }
    }
}
