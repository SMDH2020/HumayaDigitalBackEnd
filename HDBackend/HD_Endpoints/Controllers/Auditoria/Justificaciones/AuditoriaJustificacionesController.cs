using DocumentFormat.OpenXml.Office2016.Excel;
using HD.Notifications.Analisis;
using HD.Security;
using HD_Auditoria.Consultas.Justificaciones;
using HD_Auditoria.Consultas.Notificacion_Correo;
using HD_Auditoria.Consultas.Programar_Inventario;
using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Auditoria.Reporteria;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.Auditoria.Justificaciones
{
    public class AuditoriaJustificacionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AuditoriaJustificacionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> ListadoFolios()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Justificaciones_Listado datos = new AD_Justificaciones_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Listado(usuario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Listado(string? folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_JustificacionInventario_Listado datos = new AD_JustificacionInventario_Listado(CadenaConexion);
            var result = await datos.Listado(folio);
            return Ok(result);

        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Guardar([FromForm] mdl_Justificaciones_Guardar mdl)
        {
            var metadataArchivos = new List<object>();

            // Solo crear carpeta y recorrer archivos si existen
            if (mdl.archivos != null && mdl.archivos.Any())
            {
                var rutaBase = Path.Combine("C:\\ArchivosHD\\Justificaciones", mdl.folio);
                Directory.CreateDirectory(rutaBase);

                foreach (var archivo in mdl.archivos)
                {
                    var extension = Path.GetExtension(archivo.FileName)
                        .TrimStart('.')
                        .ToLower();

                    var nombreUnico = $"{Guid.NewGuid()}.{extension}";
                    var rutaFisica = Path.Combine(rutaBase, nombreUnico);
                    var rutaServidor = $"/ArchivosHD/Justificaciones/{mdl.folio}/{nombreUnico}";

                    // Guardar archivo
                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    metadataArchivos.Add(new
                    {
                        nombre = archivo.FileName,
                        tipo_archivo = extension,
                        ruta_servidor = rutaServidor,
                        tamanio_bytes = archivo.Length,
                    });
                }
            }

            var jsonMetadata = JsonConvert.SerializeObject(metadataArchivos);

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];

            AD_Justificar_Auditoria_Responsable_Almacen_Guardar datos =
                new AD_Justificar_Auditoria_Responsable_Almacen_Guardar(CadenaConexion);

            mdl.usuario = Sesion.usuario();

            var result = await datos.GuardarJustificacion(mdl, jsonMetadata);

            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> obtenerArchivos(int idjust)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_JustificacionAuditoria_ObtenerArchivos datos = new AD_JustificacionAuditoria_ObtenerArchivos(CadenaConexion);
            var result = await datos.obtenerArchivos(idjust);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> DescargarArchivos(string ruta, string nombre, string extension)
        {
            var rutaFisica = Path.Combine("C:", ruta);

            if (!System.IO.File.Exists(rutaFisica))
                return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(rutaFisica);

            var contentType = extension.ToLower() switch
            {
                "pdf" => "application/pdf",
                "jpg" => "image/jpeg",
                "jpeg" => "image/jpeg",
                "png" => "image/png",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            // File() devuelve el archivo con el Content-Type correcto
            return File(bytes, contentType, nombre);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Aceptar(mdl_Justificaciones_Acciones mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_JustificarAuditoria_Aceptar datos = new AD_JustificarAuditoria_Aceptar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.JustificacionAceptada(mdl);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> EnviarRevisionAlmacen(mdl_Justificaciones_Acciones mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Justificacion_Auditoria_Enviar_Almacen datos = new AD_Justificacion_Auditoria_Enviar_Almacen(CadenaConexion);
            var result = await datos.Correos(mdl.folio);

             await EnviarRevision.Enviar_Almacen( result, mdl.folio);

            return Ok(true);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> EnviarRevisionAuditor(mdl_Justificaciones_Acciones mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Justificacion_Auditoria_Enviar_Auditor datos = new AD_Justificacion_Auditoria_Enviar_Auditor(CadenaConexion);
            var result = await datos.Correos(mdl.folio);

            await EnviarRevision.Enviar_Auditor(result, mdl.folio);

            return Ok(true);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Rechazar(mdl_Justificaciones_Acciones mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_JustificarAuditoria_Modificar datos = new AD_JustificarAuditoria_Modificar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.JustificacionRechazar(mdl);

            if(result.estatus.rechazado == 1)
            {
                await EnvioJustificaciones.Enviar_Rechazado(result, mdl);
            }

            return Ok(result.estatus);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> obtenerTimeline(int idconteo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_JustificacionAuditoria_Timeline datos = new AD_JustificacionAuditoria_Timeline(CadenaConexion);
            var result = await datos.Timeline(idconteo);
            return Ok(result);

        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> ExtenderFechaJustificacion(mdl_ExtenderFecha mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ExtenderFechaJustificacion_Guardar datos = new AD_ExtenderFechaJustificacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.ExtenderFecha(mdl);

            if (result.estatus.finalizado == 1)
            {
                var rpt = RPT_FinalizacionMetricas.GenerarPDF(result, mdl.folio);
                byte[] pdf = Convert.FromBase64String(rpt.documento);

                await EnvioFinalizacionInventario.Enviar_Finalizacion(
                    datos_correo: result,          // mdl_Notificar_View con .correos
                    folio: mdl.folio,         // folio, fecha_limite_just, diferencias
                    pdfAdjunto: pdf,
                    nombreArchivoPdf: $"Inventario_{mdl.folio}.pdf"
                );
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> ReporteAjuste(string? folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ReporteAjuste_PDF datos = new AD_ReporteAjuste_PDF(CadenaConexion);
            var result = await datos.Listado(folio);

            var rpt = RPT_Reporte_Ajuste_PDF.GenerarPDF(result, folio);

            return Ok(rpt);

        }
    }
}
