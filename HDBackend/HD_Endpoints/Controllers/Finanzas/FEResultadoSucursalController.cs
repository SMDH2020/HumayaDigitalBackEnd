using Enlace.Dapper.Reportes;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.ResultadosSucursal;
using HD_Reporteria.Buro_Credito;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEResultadoSucursalController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEResultadoSucursalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(mdl_Resultados_Sucursal_Filtrado vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Resultados_Sucursal res = new FAD_Resultados_Sucursal(CadenaConexion);
            return Ok(await res.Obtener(vm));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(mdl_Resultados_Sucursal_Filtrado vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Resultados_Sucursal datos = new FAD_Resultados_Sucursal(CadenaConexion);
            var result = await datos.Obtener(vm);
            var docResult = await XLS_Resultados_Sucursal.CrearExel(result, vm.subtitulo);
            return Ok(docResult);
        }
    }
}
