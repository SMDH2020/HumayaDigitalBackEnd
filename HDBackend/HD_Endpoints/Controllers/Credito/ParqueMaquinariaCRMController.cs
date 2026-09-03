using HD.Clientes.Consultas.CRM.Parque_Maquinaria;
using HD.Clientes.Consultas.CRM.Reportes;
using HD.Clientes.Consultas.CRM.Visitas;
using HD.Security;
using HD_Reporteria.CRM;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ParqueMaquinariaCRMController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ParqueMaquinariaCRMController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoParqueMaquinaria(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Parque_Maquinaria datos = new AD_Parque_Maquinaria(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelParqueMaquinaria(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Parque_Maquinaria datos = new AD_Parque_Maquinaria(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(idcliente);
            var docresult = await XLS_Listado_Parque_Maquinaria.GenerarExcel(result);
            return Ok(docresult);
        }
    }
}
