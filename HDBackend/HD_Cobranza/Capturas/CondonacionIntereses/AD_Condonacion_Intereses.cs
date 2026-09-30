using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.CondonacionIntereses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.CondonacionIntereses
{
    public class AD_Condonacion_Intereses
    {
        private string CadenaConexion;
        public AD_Condonacion_Intereses(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Limite_Condonacion_Usuario> ObtenerLimiteUsuario(int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @usuario = usuario,
                };

                var result = await
                factory.SQL.QueryFirstOrDefaultAsync<mdl_Limite_Condonacion_Usuario>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Limite_Condonacion_Usuario",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Guarda_Condonacion_Interes>> Guardar(mdl_Guarda_Condonacion_Interes mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @usuario = mdl.usuario,
                    @Saldo = mdl.Saldo,
                    @Inormal_saldo = mdl.Inormal_saldo,
                    @Inormal_porcentaje = mdl.Inormal_porcentaje,
                    @Inormal_descuento = mdl.Inormal_descuento,
                    @Inormal_pagado = mdl.Inormal_pagado,
                    @Imoratorio_saldo = mdl.Imoratorio_saldo,
                    @Imoratorio_porcentaje = mdl.Imoratorio_porcentaje,
                    @Imoratorio_descuento = mdl.Imoratorio_descuento,
                    @Imoratorio_pagado = mdl.Imoratorio_pagado,
                    @Comentarios = mdl.Comentarios,
                    @facturas = mdl.facturas,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Guarda_Condonacion_Interes>("Cartera_Clientes.CondonacionInteres.sp_Guardar_Condonacion_Interes",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Condonaciones_Cliente>> ObtenerCondonacionesCliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Listado_Condonaciones_Cliente>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonaciones_Cliente",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Validar_Condonaciones_Clientes> ValidarCondonacionesCliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                };

                var result = await
                factory.SQL.QueryFirstOrDefaultAsync<mdl_Validar_Condonaciones_Clientes>("Cartera_Clientes.CondonacionInteres.sp_Validar_Condonaciones_Cliente",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Condonacion_Por_Folio_View> ObtenerCondonacionPorFolio(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @Folio = folio,
                };

                var result = await
                factory.SQL.QueryMultipleAsync("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonacion_Por_Folio",
                parametros, commandType: System.Data.CommandType.StoredProcedure);

                var view = new mdl_Condonacion_Por_Folio_View();
                view.condonacion = result.Read<mdl_Condonacion_Por_Folio>().FirstOrDefault();
                view.facturas = result.Read<mdl_Factura_Condonacion_Detalle>().ToList();

                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Autorizar_Condonacion>> Autorizar(mdl_Autorizar_Condonacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @Folio = mdl.Folio,
                    @usuario = mdl.usuario,
                    @aprobado = mdl.aprobado,
                    @Inormal_porcentaje = mdl.Inormal_porcentaje,
                    @Imoratorio_porcentaje = mdl.Imoratorio_porcentaje,
                    @ComentarioAutoriza = mdl.ComentarioAutoriza,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Autorizar_Condonacion>("Cartera_Clientes.CondonacionInteres.sp_Autorizar_Condonacion",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        private string CuerpoCorreoCondonacion(mdl_Condonacion_Por_Folio c, string tipoEvento, string? mensajeAdicional)
        {
            string estatusTexto = tipoEvento switch
            {
                "aprobada" => "Aprobada",
                "rechazada" => "Rechazada",
                _ => "Pendiente de autorización"
            };

            string colorEstatus = tipoEvento switch
            {
                "aprobada" => "#2e7d32",
                "rechazada" => "#c62828",
                _ => "#ef6c00"
            };

            string tituloEvento = tipoEvento switch
            {
                "aprobada" => "Condonación de interés aprobada",
                "rechazada" => "Condonación de interés rechazada",
                _ => "Nueva solicitud de condonación de interés"
            };

            string comentariosHtml = string.IsNullOrWhiteSpace(c.Comentarios)
                ? ""
                : $@"<p style=""margin-top:15px;""><strong>Comentarios:</strong> {c.Comentarios}</p>";

            string mensajeAdicionalHtml = string.IsNullOrWhiteSpace(mensajeAdicional)
                ? ""
                : $@"<p style=""margin-top:15px;"">{mensajeAdicional}</p>";

            return $@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <title>Condonación de interés</title>
            </head>
            <body style=""margin:0; padding:0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""padding: 20px;"">
                    <tr><td align=""center"">
                        <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff; border:1px solid #cccccc; border-radius:8px;"">
                            <tr>
                                <td style=""padding:20px;"">
                                    <h2 style=""margin-top:0;"">{tituloEvento}</h2>
                                    <p style=""margin:0 0 10px 0;"">
                                        <strong>Folio:</strong> {c.Folio} &nbsp;|&nbsp;
                                        <strong>Estatus:</strong> <span style=""color:{colorEstatus}; font-weight:bold;"">{estatusTexto}</span>
                                    </p>
 
                                    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""margin:10px 0;"">
                                        <tr>
                                            <td style=""background-color:#498c47; height:3px; border-radius:0 10px 10px 0; width:45%;""></td>
                                            <td style=""background-color:#498c47; height:3px; border-radius:10px 0 0 10px; width:45%;""></td>
                                        </tr>
                                    </table>
 
                                    <table width=""100%"" cellpadding=""6"" cellspacing=""0"" border=""0"" style=""font-size:14px;"">
                                        <tr><td><strong>Cliente:</strong></td><td>{c.razon_social}</td></tr>
                                        <tr><td><strong>Fecha:</strong></td><td>{c.createdate:dd/MM/yyyy HH:mm}</td></tr>
                                        <tr><td><strong>Solicita:</strong></td><td>{c.creador}</td></tr>
                                        <tr><td><strong>Saldo total:</strong></td><td>${c.Saldo:N2}</td></tr>
                                    </table>
 
                                    <h3 style=""margin-bottom:5px;"">Desglose por tipo de interés</h3>
                                    <table width=""100%"" cellpadding=""6"" cellspacing=""0"" border=""1"" style=""border-collapse:collapse; font-size:14px;"">
                                        <tr style=""background-color:#f0f0f0;"">
                                            <td><strong>Tipo</strong></td>
                                            <td><strong>Saldo</strong></td>
                                            <td><strong>% Condonado</strong></td>
                                            <td><strong>Descuento</strong></td>
                                            <td><strong>Interés pagado</strong></td>
                                        </tr>
                                        <tr>
                                            <td>Normal</td>
                                            <td>${c.Inormal_saldo:N2}</td>
                                            <td>{c.Inormal_porcentaje}%</td>
                                            <td>${c.Inormal_descuento:N2}</td>
                                            <td>${(c.Inormal_saldo - c.Inormal_descuento):N2}</td>
                                        </tr>
                                        <tr>
                                            <td>Moratorio</td>
                                            <td>${c.Imoratorio_saldo:N2}</td>
                                            <td>{c.Imoratorio_porcentaje}%</td>
                                            <td>${c.Imoratorio_descuento:N2}</td>
                                            <td>${(c.Imoratorio_saldo - c.Imoratorio_descuento):N2}</td>
                                        </tr>
                                    </table>
 
                                    {comentariosHtml}
                                    {mensajeAdicionalHtml}
                                </td>
                            </tr>
                        </table>
                    </td></tr>
                </table>
            </body>
            </html>";
        }

        // tipoEvento: "solicitud" | "aprobada" | "rechazada"
        // correosCsv: correos separados por coma, ej. "a@x.com,b@x.com" (ya resueltos por el stored)
        public async Task<bool> EnviarCorreoCondonacion(
            string folio,
            string tipoEvento,
            string? correosCsv,
            string smtpHost,
            int smtpPort,
            bool smtpSecure,
            string smtpUser,
            string smtpPass,
            string? mensajeAdicional = null)
        {
            try
            {
                var destinatarios = (correosCsv ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct()
                    .ToList();

                if (!destinatarios.Any())
                    return false;

                var vista = await ObtenerCondonacionPorFolio(folio);

                if (vista?.condonacion == null)
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "No se encontró la condonación." });

                using var objeto_mail = new MailMessage();
                using var client = new SmtpClient();
                client.Port = smtpPort;
                client.Host = smtpHost;
                client.Timeout = 20000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = smtpSecure;

                objeto_mail.From = new MailAddress(smtpUser);
                foreach (var correo in destinatarios)
                    objeto_mail.To.Add(new MailAddress(correo));

                objeto_mail.Subject = tipoEvento switch
                {
                    "aprobada" => $"Condonación {folio} aprobada",
                    "rechazada" => $"Condonación {folio} rechazada",
                    _ => $"Nueva solicitud de condonación {folio}"
                };
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = CuerpoCorreoCondonacion(vista.condonacion, tipoEvento, mensajeAdicional);

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

        public async Task<IEnumerable<mdl_Condonacion_Reporte>> ObtenerCondonacionesReporte(int ejercicio, int periodo, string adr, string sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @ejercicio = ejercicio,
                    @periodo = periodo,
                    @adr = adr,
                    @sucursal = sucursal,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Condonacion_Reporte>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonaciones_Reporte",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
