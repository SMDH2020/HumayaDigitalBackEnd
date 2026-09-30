using Enlace.Dapper.Reportes;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.AntiguedadInventario;
using HD_Reporteria.Buro_Credito;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEInventarioAntiguedadController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEInventarioAntiguedadController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerInventario(mdl_vInventario vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Inventario_Antiguedad inv = new FAD_Inventario_Antiguedad(CadenaConexion);
            return Ok(await inv.ObtenerInventario(vm));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(mdl_vInventario vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Inventario_Antiguedad datos = new FAD_Inventario_Antiguedad(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerInventario(vm);
            var docResult = await XLS_Antiguedad_Inventario.CrearExel(result, vm, usuario.ToString());
            return Ok(docResult);
        }
    }
}
