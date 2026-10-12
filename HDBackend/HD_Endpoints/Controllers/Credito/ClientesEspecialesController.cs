using HD.Clientes.Consultas.ClientesDocumentacion;
using HD.Clientes.Consultas.Especiales;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Especiales;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesEspecialesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesEspecialesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlClientesEspeciales mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarDocumento(mdl_Clientes_Especiales_Documento mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.GuardarDocumento(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Documento(string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.Documento(tipo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.Listado(tipo);
            return Ok(result);
        } 

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList(string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.DropDownList(tipo);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownListTodos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.DropDownListTodos();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerInfoCliente(int idCliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.InfoCliente(idCliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.Listado(tipo);
            var docresult = await XLSCre_Listado_Clientes_Especiales.GenerarExcel(result);
            return Ok(docresult);
        }
    }
}
