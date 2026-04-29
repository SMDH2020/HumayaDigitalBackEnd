using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado
{
    public class ADSolicitud_Credito_Documentacion_JDF_Guardar
    {
        private string CadenaConexion;
        public ADSolicitud_Credito_Documentacion_JDF_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_CRedito_Documentacion_Email> Guardar(mdlSolicitudCredito_Documentacion_View view)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = view.folio,
                    iddocumento = view.iddocumento,
                    documento = view.documento,
                    comentarios = view.comentarios,
                    extension = view.extension,
                    vigencia = view.vigencia,
                    usuario = view.usuario,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documentacion_JDF_Guardar_Email", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitud_CRedito_Documentacion_Email documentocargados = new mdlSolicitud_CRedito_Documentacion_Email();
                documentocargados.documentacion = result.Read<mdlSolicitudCredito_Documentacion>().ToList();
                documentocargados.notificar = result.Read<mdl_Notificar>().FirstOrDefault();
                documentocargados.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();


            //    // Verificar si notificar es 0
            //    if (documentocargados.notificar != null && documentocargados.notificar.notificar == 0)
            //    {
            //        // Crear un solo objeto mdlSolicitud con idusuario igual a 0
            //        documentocargados.mdlSolicitud = new List<mdlSolicitudCredito_Enviar> 
            //{
            //            new mdlSolicitudCredito_Enviar { 
            //                idempleado = 0,
            //                nombre = "",
            //                correo = ""
            //            }

            //};
            //    }

                factory.SQL.Close();
                return documentocargados;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlSolicitud_CRedito_Documentacion_Email> GuardarIMGtoPDF(string folio, int iddocumento, string documento, string comentarios, string extension, string vigencia, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = folio,
                    iddocumento = iddocumento,
                    documento = documento,
                    comentarios = comentarios,
                    extension = extension,
                    vigencia = vigencia,
                    usuario = usuario,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documentacion_JDF_Guardar_Email", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitud_CRedito_Documentacion_Email documentocargados = new mdlSolicitud_CRedito_Documentacion_Email();
                documentocargados.documentacion = result.Read<mdlSolicitudCredito_Documentacion>().ToList();
                documentocargados.notificar = result.Read<mdl_Notificar>().FirstOrDefault();
                documentocargados.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();


                // Verificar si notificar es 0
                if (documentocargados.notificar != null && documentocargados.notificar.notificar == 0)
                {
                    // Crear un solo objeto mdlSolicitud con idusuario igual a 0
                    documentocargados.mdlSolicitud = new List<mdlSolicitudCredito_Enviar>
            {
                        new mdlSolicitudCredito_Enviar {
                            idempleado = 0,
                            nombre = "",
                            correo = ""
                        }

            };
                }

                factory.SQL.Close();
                return documentocargados;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlSolicitud_CRedito_Documentacion_Email> GuardarDocumentoReestructuracionNotificar(mdlSolicitudCredito_Documentacion_View view)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = view.folio,
                    iddocumento = view.iddocumento,
                    documento = view.documento,
                    comentarios = view.comentarios,
                    extension = view.extension,
                    vigencia = view.vigencia,
                    usuario = view.usuario,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documentacion_Reestructuracion_Guardar_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitud_CRedito_Documentacion_Email documentocargados = new mdlSolicitud_CRedito_Documentacion_Email();
                documentocargados.documentacion = result.Read<mdlSolicitudCredito_Documentacion>().ToList();
                documentocargados.notificar = result.Read<mdl_Notificar>().FirstOrDefault();
                documentocargados.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();


                // Verificar si notificar es 0
                if (documentocargados.notificar != null && documentocargados.notificar.notificar == 0)
                {
                    // Crear un solo objeto mdlSolicitud con idusuario igual a 0
                    documentocargados.mdlSolicitud = new List<mdlSolicitudCredito_Enviar>
            {
                        new mdlSolicitudCredito_Enviar {
                            idempleado = 0,
                            nombre = "",
                            correo = ""
                        }

            };
                }

                factory.SQL.Close();
                return documentocargados;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlSolicitudCredito_Documentacion>> GuardarDocumentoReestructuracion(mdlSolicitudCredito_Documentacion_View view)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = view.folio,
                    iddocumento = view.iddocumento,
                    documento = view.documento,
                    comentarios = view.comentarios,
                    extension = view.extension,
                    vigencia = view.vigencia,
                    usuario = view.usuario,
                };
                IEnumerable<mdlSolicitudCredito_Documentacion> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Documentacion>("Credito.sp_Solicitud_Credito_Documentacion_Reestructuracion_Guardar_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
