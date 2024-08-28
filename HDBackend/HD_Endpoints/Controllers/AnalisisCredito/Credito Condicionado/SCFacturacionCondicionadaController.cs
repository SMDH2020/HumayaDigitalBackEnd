using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCFacturacionCondicionadaController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCFacturacionCondicionadaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_Autorizar_facturacion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            mdl.usuario = Sesion.usuario();
            AD_Credito_Autorizar_Facturacion_Condicionada da = new AD_Credito_Autorizar_Facturacion_Condicionada(CadenaConexion);
            var result = await da.Guardar(mdl);

            return Ok(result);

        }
    }
}
