using HD.Security;
using HD_CentroMonitoreo.Consultas;
using HD_CentroMonitoreo.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CentroMonitoreo
{
    public class CatalogoAlertasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CatalogoAlertasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCatalogoAlertas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_Alertas datos = new AD_Catalogo_Alertas(CadenaConexion);
            var result = await datos.ObtenerCatalogo();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoAlertasDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_Alertas datos = new AD_Catalogo_Alertas(CadenaConexion);
            var result = await datos.ListadoAlertasDDL();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCatalogoAlerta(mdl_Guardar_Catalogo_Alerta mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_Alertas datos = new AD_Catalogo_Alertas(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var id_alerta = await datos.GuardarAlerta(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
                id_alerta = id_alerta
            });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarCatalogoAlerta(int id_alerta)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_Alertas datos = new AD_Catalogo_Alertas(CadenaConexion);
            await datos.EliminarAlerta(id_alerta);

            return Ok(new
            {
                mensaje = "Eliminado Correctamente",
            });
        }
    }
}
