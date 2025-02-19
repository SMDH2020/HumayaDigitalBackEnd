using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Consultas.ClientesCultivo;
using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Consultas.Refacturacion_Credito;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using HD_Reporteria.Usados;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Inventario;
using Usados.Modelos.Inventario;
using Usados.Modelos.Usados;

namespace HD.Endpoints.Controllers.Usados.Inventario
{
    public class InventarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InventarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarPrecio(mdl_Listado_Precio mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Guardar datos = new AD_Listado_Precio_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.ActualizarPrecio(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarTodosPrecio(mdl_datosActualizados mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            var usuario = Sesion.usuario();
            AD_Listado_Precio_Guardar datos_documentos = new AD_Listado_Precio_Guardar(CadenaConexion);
            foreach (mdl_Listado_Precio data in mdl.datosActualizados)
            {
                await datos_documentos.ActualizarTodosPrecio(data.idinventario, data.utilidad, data.margen, data.precio_lista, data.usuario);
            }

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string Modelo, int ejercicio, string HP, string Sucursal, string Promocion, string Estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.Listado(Modelo, ejercicio, HP, Sucursal, Promocion, Estatus);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoFiltro()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();

            try
            {
                RPT_Result documento = RPT_Listado_Precios.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarPromocion(mdl_promocion mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Guardar datos = new AD_Listado_Precio_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.GuardarPromocion(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerPromocion( int idinventario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Obtener_Promocion datos = new AD_Listado_Precio_Obtener_Promocion(CadenaConexion);
            var result = await datos.BuscarID(idinventario);
            return Ok(result);

        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BorrarPromocion(int idpromocion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Precio_Promocion_Borrar datos = new AD_Listado_Precio_Promocion_Borrar(CadenaConexion);
            var result = await datos.Borrar(idpromocion);
            return Ok(result);

        }
    }
}
