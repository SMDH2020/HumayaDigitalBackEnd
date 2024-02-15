using HD.Security;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.Gastos_Proyeccion;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEGastos_ConceptoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEGastos_ConceptoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GastosvsProyeccionbyTiempo(Fmdl_Gastos_Filtros vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_GastosvsProyeccion datos = new FAD_GastosvsProyeccion(CadenaConexion);
            var result = await datos.GetGastosvsProyeccion(vm, "1");
            return Ok(result);
        }
    }
}
