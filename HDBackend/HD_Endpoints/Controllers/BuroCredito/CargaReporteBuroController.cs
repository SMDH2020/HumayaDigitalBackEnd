using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaReporteBuroController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaReporteBuroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar(int ejercicio, int periodo, int sucursal, string mostrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Reporte_Buro datos = new AD_Carga_Reporte_Buro(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            usuario = 8919;
            var result = await datos.reporte(ejercicio, periodo, sucursal, mostrar);
            return Ok(result);
        }
    }
}
