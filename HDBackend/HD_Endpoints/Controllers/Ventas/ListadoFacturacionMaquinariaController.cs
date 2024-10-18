using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Security;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ListadoFacturacionMaquinariaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoFacturacionMaquinariaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Lineas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_DropDownList_Lineas datos = new AD_DropDownList_Lineas(CadenaConexion);
            var result = await datos.Lineas();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Parametros(int ejercicio, int periodo, string adr, string sucursal, int linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Facturacion_Maquinaria datos = new AD_Listado_Facturacion_Maquinaria(CadenaConexion);
            var result = await datos.Get(ejercicio, periodo, adr, sucursal, linea);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Editar(int id, string nip, string modelo, string usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Editar_Modelo_Facturacion_Maquinaria datos = new AD_Editar_Modelo_Facturacion_Maquinaria(CadenaConexion);
            usuario = Sesion.usuario();
            var result = await datos.Editar(id, nip, modelo, usuario);
            return Ok(result);
        }
    }
}
