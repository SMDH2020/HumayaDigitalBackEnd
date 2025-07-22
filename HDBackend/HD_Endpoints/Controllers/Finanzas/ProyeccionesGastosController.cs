using Enlace.Dapper.Reportes;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.NivelInventario;
using HD_Finanzas.Modelos.ProyeccionesGastos;
using HD_Finanzas.Modelos.ProyeccionesVentas;
using HD_Reporteria.Buro_Credito;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class ProyeccionesGastosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ProyeccionesGastosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(mdl_Filtro_Proyecciones_Gastos vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proyeccion_Gastos nvl = new AD_Proyeccion_Gastos(CadenaConexion);
            string usuario = Sesion.usuario();
            return Ok(await nvl.Obtener(vm, usuario));
        }
    }
}
