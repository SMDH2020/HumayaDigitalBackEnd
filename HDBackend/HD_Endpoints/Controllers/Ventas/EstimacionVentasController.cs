using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class EstimacionVentasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public EstimacionVentasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerVentasEstimadas(bool bylineas,int anio, int periodo, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_EstimacionVentas datos = new AD_EstimacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerVentasEstimadas(bylineas,anio, periodo, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerEstimacionesVentasSucursal(int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_EstimacionVentas datos = new AD_EstimacionVentas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerEstimacionesVentasSucursal(sucursal, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarEstimacionVentas(mdl_GuardarEstimacionVentas mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_EstimacionVentas datos = new AD_EstimacionVentas(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.GuardarEstimacionVentas(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            });
        }
    }
}