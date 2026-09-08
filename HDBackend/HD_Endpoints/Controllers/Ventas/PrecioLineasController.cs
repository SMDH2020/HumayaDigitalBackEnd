using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using HD_Ventas.Consultas.PrecioLineas;
using HD_Ventas.Modelos.PrecioLista;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas;
using Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class PrecioLineasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PrecioLineasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdl_Listado_Modificado_Precios_Linea mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Precio_Linea_Guardar datos = new AD_Precio_Linea_Guardar(CadenaConexion);
            var usuario = Sesion.usuario();
            foreach (mdl_Precio_Linea_Guardar fac in mdl.preciosLinea)
            {
                await datos.Guardar_precio(fac.idlinea, fac.ejercicio, fac.periodo, fac.sucursal, fac.precio, usuario);
            }
            return Ok(new
            {
                mensaje = "Guardado Correctamente"
            });
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int ejercicio, int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Precio_Lineas_Listado datos = new AD_Precio_Lineas_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, sucursal);
            return Ok(result);

        }
    }
}
