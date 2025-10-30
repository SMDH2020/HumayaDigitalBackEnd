using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Postventa.Consultas.ReporteMensajeria;
using Postventa.Modelos;

namespace HD.Endpoints.Controllers.Postventa
{
    public class DashboardPostventaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashboardPostventaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FiltroSucursalesDash()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Postventa_Info datos = new AD_Dashboard_Postventa_Info(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerFiltroSucursal(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Dashboard(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Postventa_Info datos = new AD_Dashboard_Postventa_Info(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerDashboard(ejercicio, periodo_inicio, periodo_fin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> VencimientosGarantias(int ejercicio, int periodo_inicio, int periodo_fin,string facturado, string whatsapp, string estado, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerVencimientos(ejercicio, periodo_inicio, periodo_fin,facturado, whatsapp, estado, adr, sucursal, usuario);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarNumeroClienteGarantia(Int64 numero, int idgarantia)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.ActualizarNumero(numero, idgarantia);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PreciosGarantias()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.ObtenerPrecios();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarPreciosGarantias(mdl_Cargar_Precios_Garantia mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            foreach (mdl_Datos_Carga_Precios_Garantia info in mdl.datos)
            {
                info.fecha_inicio = mdl.inicio_vigencia;
                info.fecha_fin = mdl.vigencia;
                info.tipo_carga = mdl.tipo_carga;
                await datos.cargarInformacion(info);
            }
            var result = await datos.ObtenerPrecios();
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerPreciosGarantias(int id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.obtenerID(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ModelosGarantia()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.ObtenerModelos();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarPrecioGarantia(mdl_Precios_Garantias_porModelo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.ActualizarPrecioGarantia(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarPrecioGarantia(mdl_Agregar_Precio_Garantia mdl) //no se utiliza
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarPrecioGarantia(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MensajeGarantias(string tipo) //no se utiliza
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.ObtenerMensaje(tipo);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarMensajeGarantia(mdl_Mensaje_Garantia mdl) //no se utiliza
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarMensajeGarantia(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CotizacionesAbiertas(int ejercicio, int periodo_inicio, int periodo_fin,string facturado, string whatsapp, string estado, string motivo, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Cotizaciones_Abiertas datos = new AD_Obtener_Cotizaciones_Abiertas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerCotizaciones(ejercicio, periodo_inicio, periodo_fin,facturado, whatsapp, estado, motivo, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ExcluirModelo(string modelo, string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ExcluirModelo(modelo, tipo, usuario);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarReglaExclusion(string id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.EliminarReglaExclusion(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ServiciosPendientes(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal, int hrsuso, string msj_estatus, string motivo, string facturado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerServicios(ejercicio, periodo_inicio, periodo_fin, adr, sucursal, hrsuso, msj_estatus, motivo, facturado, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarContactoServiciosPendientes(mdl_Agregar_Contacto_Servicios_Pendientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarContactoServiciosPendientes(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PaquetesmantenimientoDisponibles()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            var result = await datos.ObtenerPaquetesMantenimiento();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetPaqueteEditar(int id_paquete)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            var result = await datos.ObtenerPaquetesMantenimientoid(id_paquete);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarPaqueteMantenimiento(mdl_Paquetes_Mantenimiento mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarPaqueteMantenimiento(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetPreciosMantenimiento(int id_paquete)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            var result = await datos.ObtenerPreciosMantenimiento(id_paquete);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetPrecioModeloMantenimiento(int id_precio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            var result = await datos.ObtenerPrecioMantenimientoModelo(id_precio);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarPreciosMantenimiento(mdl_Precios_Mantenimiento_porModelo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Servicios_Pendientes datos = new AD_Obtener_Servicios_Pendientes(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.GuardarPreciosMantenimiento(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }
    }
}
