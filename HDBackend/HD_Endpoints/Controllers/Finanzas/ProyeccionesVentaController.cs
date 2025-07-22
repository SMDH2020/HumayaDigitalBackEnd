using Enlace.Dapper.Reportes;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.NivelInventario;
using HD_Finanzas.Modelos.ProyeccionesVentas;
using HD_Reporteria.Buro_Credito;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class ProyeccionesVentaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ProyeccionesVentaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(mdl_Filtro_Proyecciones_Ventas vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ProyeccionesVentas nvl = new AD_ProyeccionesVentas(CadenaConexion);
            string usuario = Sesion.usuario();
            return Ok(await nvl.ObtenerProyeccion(vm, usuario));
        }
    }
}
