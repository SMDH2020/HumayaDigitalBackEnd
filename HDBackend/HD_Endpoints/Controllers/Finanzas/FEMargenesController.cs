using HD_Finanzas.AccesoDatos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.Margenes;
using Enlace.Dapper.Reportes;
using DocumentFormat.OpenXml.EMMA;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEMargenesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEMargenesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetMargenes(mdlERMargenes vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Margenes margenes = new FAD_Margenes(CadenaConexion);
            string usuario = Sesion.usuario();
            usuario = "1";
            return Ok(await margenes.GetMargenes(vm, usuario));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetMargenesDetalle(mdlMargenes_Detalle vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            vm.usuario = Sesion.usuario();
            vm.usuario = "15";
            FAD_Margenes margenes = new FAD_Margenes(CadenaConexion);
            var result = await margenes.GetMargenesDetalle(vm);
            return Ok(result);
        }
    }
}
