using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos.CRM;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Cotizaciones
{
    public class AD_Cotizaciones_CRM
    {
        private string CadenaConexion;
        public AD_Cotizaciones_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<string> Guardar(mdl_Cotizaciones_CRM_Guarad mdl)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, DbType.StringFixedLength, ParameterDirection.InputOutput, size: 13);
                parametros.Add("@datos", mdl.datos, DbType.String);
                parametros.Add("@usuario", mdl.usuario, DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Cotizaciones_Servicio_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return parametros.Get<string>("@folio");
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> Eliminar(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Cotizaciones_Servicio_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Listado_Cotizaciones_CRM_View> Listado(string fechainicio, string fechafin, string adr, string sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    adr = adr,
                    sucursal = sucursal,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Listado_Cotizaciones_ServicioCRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Listado_Cotizaciones_CRM_View mdl = new mdl_Listado_Cotizaciones_CRM_View();
                mdl.cotizaciones = result.Read<mdl_Listado_Cotizaciones_CRM>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();

                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Cotizaciones_CRM_Folio_View> ObtenerPorFolio(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Cotizaciones_ServicioCRM_folio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Cotizaciones_CRM_Folio_View mdl = new mdl_Cotizaciones_CRM_Folio_View();
                mdl.Clientes = result.Read<mdl_Opciones_Clientes_Cotizacion_CRM>().ToList();
                mdl.Asesores = result.Read<mdl_Opciones_Asesores_Cotizaciones_CRM>().ToList();
                mdl.Origenes = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.TiposPago = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.Cotizacion = result.Read<mdl_Cotizaciones_CRM_Folio>().FirstOrDefault();
                mdl.Detalle = result.Read<mdl_Cotizaciones_CRM_Folio_Detalle>().ToList();
                mdl.caracteristicas = result.Read<mdl_Cotizaciones_CRM_Folio_Caracteristicas>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();

                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        // Genera el PDF de la cotización (mismo generador que usa ImprimirPDF)
        // y lo envía por correo, adjunto, a los destinatarios indicados.
        public async Task<bool> EnviarPorCorreo(string folio, string plantilla, int usuario, IEnumerable<string> destinatarios, string mensajeAdicional)
        {
            try
            {
                var vista = await ObtenerPorFolio(folio, usuario);

                if (vista?.Cotizacion == null)
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "No se encontró la cotización." });

                var correosValidos = (destinatarios ?? Enumerable.Empty<string>())
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .Select(d => d.Trim())
                    .ToList();

                if (!correosValidos.Any())
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "Debes indicar al menos un destinatario." });

                RPT_Result documento = RPT_Cotizacion_CRM.GenerarPDF(vista, plantilla);
                byte[] pdfBytes = Convert.FromBase64String(documento.documento);

                // Se reutiliza el mismo Mapear que usa GenerarPDF (público) para
                // poder armar el cuerpo del correo con los mismos datos del PDF.
                var c = RPT_Cotizacion_CRM.Mapear(vista);

                string password = "!HD_Hum4y4D1g1t4l*T1?";
                string _correo = "HumayaDigital@humaya.com.mx";

                using var objeto_mail = new MailMessage();
                using var client = new SmtpClient();
                client.Port = 587;
                client.Host = "correo.humaya.com.mx";
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_correo, password);
                client.EnableSsl = false;

                objeto_mail.From = new MailAddress(_correo);
                foreach (var destinatario in correosValidos)
                    objeto_mail.To.Add(new MailAddress(destinatario));

                objeto_mail.Subject = $"Cotización {c.folio_crm}" +
                    (string.IsNullOrWhiteSpace(c.asunto) ? "" : $" - {c.asunto}");
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = CuerpoCorreoCotizacion(c, mensajeAdicional);

                using var streamPdf = new MemoryStream(pdfBytes);
                var nombreArchivo = $"{documento.nombredocumento}_{c.folio_crm}.pdf";
                objeto_mail.Attachments.Add(new Attachment(streamPdf, nombreArchivo, "application/pdf"));

                client.Send(objeto_mail);
                return true;
            }
            catch (Excepciones)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        private static string CuerpoCorreoCotizacion(mdl_Cotizacion_CRM_Imprimir c, string mensajeAdicional)
        {
            string mensaje = string.IsNullOrWhiteSpace(mensajeAdicional)
                ? "Adjunto encontrarás la cotización solicitada."
                : mensajeAdicional;

            return "<html><body style=\"font-family:Calibri, Arial, sans-serif; font-size:14px; color:#333;\">" +
                   $"<p>Estimado(a) <strong>{c.apreciable}</strong>,</p>" +
                   $"<p>{mensaje}</p>" +
                   "<p>" +
                   $"<strong>Folio de cotización:</strong> {c.folio_crm}<br/>" +
                   (string.IsNullOrWhiteSpace(c.asunto) ? "" : $"<strong>Asunto:</strong> {c.asunto}<br/>") +
                   $"<strong>Vigencia:</strong> {c.vigencia}" +
                   "</p>" +
                   "<p>Cualquier duda quedamos a tus órdenes.</p>" +
                   $"<p>Saludos,<br/>{c.asesorventa}<br/>Maquinaria del Humaya, S.A. de C.V.</p>" +
                   "</body></html>";
        }


        public async Task<IEnumerable<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM>> GetOpcionesCorreo(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM> result = await factory.SQL.QueryAsync<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM>("CRM.Listado_Correos_Cliente_Cotizaciones_Servicio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}