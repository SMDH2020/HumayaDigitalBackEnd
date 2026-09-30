using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado;
using HD.Clientes.Consultas.Facturar_Equipo;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF_Condicionado
{
    public class SCJFacturacionController:MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCJFacturacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturacion_Equipo_JDF datos = new ADFacturacion_Equipo_JDF(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            //foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            //{
            //    await datos.Guardar_detalle(mdl.folio, mdl.registro, fac.orden, fac.documento, mdl.usuario, fac.docto_financiamiento);
            //}
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> informacion_pedido(string folio)
        {
            if (folio == null || folio.Length != 13)
            {
                return BadRequest("la información proporcionada no es correcta");
            }
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADFacturacion_Equipo_JDF datos = new ADFacturacion_Equipo_JDF(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.informacion(folio, usuario);
            return Ok(result);

        }
    }
}
