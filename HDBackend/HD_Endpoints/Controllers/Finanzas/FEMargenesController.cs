using HD_Finanzas.AccesoDatos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.Margenes;
using Enlace.Dapper.Reportes;
using DocumentFormat.OpenXml.EMMA;
using HD_Cobranza.Consultas.Juridico;
using HD_Ventas.Reportes;
using HD_Reporteria.Finanzas.Excel;

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

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MargenesBrutos(int ejercicio, string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Margenes_Brutos datos = new FAD_Margenes_Brutos(CadenaConexion);
            var result = await datos.GetMargenesBrutos(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MargenesBrutosExcel(int ejercicio, string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_Margenes_Brutos datos = new FAD_Margenes_Brutos(CadenaConexion);
            var result = await datos.GetMargenesBrutos(ejercicio, periodo);
            var docresult = await XLS_Margenes_Brutos.GenerarExcel(result, ejercicio, periodo);
            return Ok(docresult);
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
