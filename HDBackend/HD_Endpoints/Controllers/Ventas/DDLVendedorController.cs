using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas;
using Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class DDLVendedorController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DDLVendedorController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        //[HttpGet]
        //[Route("/api/[controller]/[action]")]
        //public async Task<ActionResult> Listado()
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    AD_DDLVendedor datos = new AD_DDLVendedor(CadenaConexion);
        //    var result = await datos.Listado();
        //    return Ok(result);

        //}
    }
}
