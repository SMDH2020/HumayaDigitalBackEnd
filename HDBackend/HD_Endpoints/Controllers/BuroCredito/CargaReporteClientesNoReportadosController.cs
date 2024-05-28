using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Buro.Modelos;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaReporteClientesNoReportadosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaReporteClientesNoReportadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar(int ejercicio, int periodo, int sucursal, string mostrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Clientes_NoReportados datos = new AD_Carga_Clientes_NoReportados(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            usuario = 8919;
            var result = await datos.reporte(ejercicio, periodo, sucursal, mostrar);
            return Ok(result);
        }
    }
}
