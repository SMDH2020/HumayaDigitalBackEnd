using Enlace.Dapper.Reportes;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.CostoFinanciamiento;
using HD_Reporteria.Buro_Credito;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FECostoFinanciamientoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FECostoFinanciamientoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(mdl_Costo_Financiamiento_Filtrado vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Costo_Financiamiento fin = new FAD_Costo_Financiamiento(CadenaConexion);
            return Ok(await fin.Obtener(vm));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(mdl_Costo_Financiamiento_Filtrado vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Costo_Financiamiento datos = new FAD_Costo_Financiamiento(CadenaConexion);
            var result = await datos.Obtener(vm);
            var docResult = await XLS_Costo_Financiamiento.CrearExel(result, vm);
            return Ok(docResult);
        }
    }
}
