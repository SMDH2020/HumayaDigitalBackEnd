using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using ProductoAliado.Modelos.Inventario;
using HD_Reporteria;
using HD_Reporteria.Cotizaciones;
using HD.AccesoDatos;


namespace HD.Endpoints.Controllers.Ventas
{
    public class CotizacionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cotizaciones(int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Cotizaciones datos = new AD_Listado_Cotizaciones(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AsesoresDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Cotizacion datos = new AD_Agregar_Cotizacion(CadenaConexion);
            var result = await datos.ListadoAsesores();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ClientesSearch()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Cotizacion datos = new AD_Agregar_Cotizacion(CadenaConexion);
            var result = await datos.ListadoClientes();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> SucursalesDDL()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Cotizacion datos = new AD_Agregar_Cotizacion(CadenaConexion);
            var result = await datos.ListadoSucursales();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarCotizacion(mdl_Agregar_Cotizacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Agregar_Cotizacion datos = new AD_Agregar_Cotizacion(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AgregarCotizacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CotizacionesDetalle(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Cotizaciones datos = new AD_Listado_Cotizaciones(CadenaConexion);
            var result = await datos.Detalle(folio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CotizacionDetalleFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Cotizaciones datos = new AD_Listado_Cotizaciones(CadenaConexion);
            var result = await datos.DetalleCotizacion(folio);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ModificarCotizacion(mdl_Modificar_Cotizacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modificar_Cotizacion datos = new AD_Modificar_Cotizacion(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.ModificarCotizacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Cotizaciones datos = new AD_Listado_Cotizaciones(CadenaConexion);
            var result = await datos.ImprimirCotizacion(folio);

            try
            {
                RPT_Result documento = RPT_Cotizacion.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetRol()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Cotizaciones datos = new AD_Listado_Cotizaciones(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GetRol(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarFase(string folio, string fase)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modificar_Cotizacion datos = new AD_Modificar_Cotizacion(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ModificarFase(folio, fase, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetFase(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modificar_Cotizacion datos = new AD_Modificar_Cotizacion(CadenaConexion);
            var result = await datos.GetFase(folio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerSolicitudes(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Solicitudes datos = new AD_Obtener_Solicitudes(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarFacturacion(mdl_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guardar_Facturacion_Cotizacion datos = new AD_Guardar_Facturacion_Cotizacion(CadenaConexion);

            mdl.usuario = Sesion.usuario();
            var result = await datos.ModificarCotizacion(mdl);


            if (mdl.entregado == "S")
            {

                // ✅ Crear la lista
                List<mdl_Envio_Whatsapp> lista = new List<mdl_Envio_Whatsapp>();

                if (mdl.contacto_servicio != mdl.contacto_refacciones)
                {
                    lista.Add(new mdl_Envio_Whatsapp()
                    {
                        telefono = mdl.contacto_servicio,
                        nombre = "result"
                    });

                    lista.Add(new mdl_Envio_Whatsapp()
                    {
                        telefono = mdl.contacto_refacciones,
                        nombre = "result"
                    });
                }
                else
                {
                    lista.Add(new mdl_Envio_Whatsapp()
                    {
                        telefono = mdl.contacto_servicio,
                        nombre = "result"
                    });
                }

                var data = new
                {
                    usuarios = lista,
                    options = new
                    {
                        contentSid = "HXe9ac52eca337f1b93acbc67f4d00a9a8",
                        coleccion = "sesiones-chatbot-central",
                        campos = new string[] { "nombre" }
                    }
                };

                string respuestaServicio = await HD_Ventas.Consultas.Conexion_Servicio_Mensajeria.send("enviar_mensajes", data);
            }

            return Ok(new
            {
                mensaje = "Guardado Correctamente"
            });
        }

    }
}
