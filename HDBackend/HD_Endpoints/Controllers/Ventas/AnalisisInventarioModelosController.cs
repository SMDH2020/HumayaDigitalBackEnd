using HD.Security;
using HD_Ventas.Consultas.AnalisisInventarioModelos;
using HD_Ventas.Consultas.ObjetivosModelos;
using HD_Ventas.Modelos.AnalisisInventarioModelos;
using HD_Ventas.Modelos.ObjetivosModelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class AnalisisInventarioModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AnalisisInventarioModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        // Los rangos van como periodo AAAAMM (ej. 202601 = enero 2026) y pueden cruzar de año.
        // comparativo: año contra el que se compara. sucursal y adr: ids separados por coma ("1,5,8"); "0" = todas
        public async Task<ActionResult> Reporte(int inicio_tendencia, int fin_tendencia, int inicio_resto, int fin_resto, int inicio_proyeccion, int fin_proyeccion, int comparativo, string sucursal = "0", string adr = "0")
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_AnalisisInventarioModelos_Reporte datos = new AD_AnalisisInventarioModelos_Reporte(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Reporte(inicio_tendencia, fin_tendencia, inicio_resto, fin_resto, inicio_proyeccion, fin_proyeccion, comparativo, sucursal, adr, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarProyeccionPedidos(mdl_AnalisisInventarioModelos_ProyeccionPedidos_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_AnalisisInventarioModelos_ProyeccionPedidos_Guardar datos = new AD_AnalisisInventarioModelos_ProyeccionPedidos_Guardar(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.Guardar(mdl, usuario);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarRelacionModelos(mdl_AnalisisInventarioModelos_RelacionModelos_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_AnalisisInventarioModelos_RelacionModelos_Guardar datos = new AD_AnalisisInventarioModelos_RelacionModelos_Guardar(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.Guardar(mdl, usuario);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }
    }
}
