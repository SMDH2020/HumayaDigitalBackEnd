using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Postventa.Consultas.ReporteMensajeria;
using Postventa.Modelos;

namespace HD.Endpoints.Controllers.Postventa
{
    public class MensajeriaGeneralController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MensajeriaGeneralController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FiltroSucursales()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Filtro_Sucursales_Rol datos = new AD_Filtro_Sucursales_Rol(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerFiltroSucursal(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerMensajeria(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string adr, string sucursal, string mostrar, string interes, string motivo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Mensajeria_General_Postventas datos = new AD_Mensajeria_General_Postventas(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.ObtenerReporte(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin, adr, sucursal, mostrar, interes, motivo, usuario);
            return Ok(result);
        }
    }
}
