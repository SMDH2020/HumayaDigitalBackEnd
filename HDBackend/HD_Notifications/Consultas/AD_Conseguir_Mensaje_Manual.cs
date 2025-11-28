using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Consultas
{
    public class AD_Conseguir_Mensaje_Manual
    {
        private string CadenaConexion;
        public AD_Conseguir_Mensaje_Manual(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo> obtenerID(int idencabezado, DateTime fecha_evento, string? usuario)
        {
            try
            {
                var parametros = new
                {
                    idencabezado = idencabezado,
                    fecha = fecha_evento,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo>("HumayaDigital_Eventos.dbo.Obtener_Mensaje_Push_One_Signal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Notificacion_Usuarios_Especificos_View>  obtenerIDUsuario(int idencabezado, DateTime fecha_evento, string? usuario, string? usuarioNotificar)
        {
            try
            {
                var parametros = new
                {
                    idencabezado = idencabezado,
                    fecha = fecha_evento,
                    usuario = usuario,
                    usuarioNotificar = usuarioNotificar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var result = await factory.SQL.QueryMultipleAsync(", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var result = await factory.SQL.QueryMultipleAsync("HumayaDigital_Eventos.dbo.Obtener_Mensaje_Push_Especifico", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Notificacion_Usuarios_Especificos_View view = new mdl_Notificacion_Usuarios_Especificos_View();
                view.notificacionCuerpo = result.Read<mdl_HD_Notificaciones_Usuario_Especifico>().FirstOrDefault();
                view.notificacionUsuarios = result.Read<mdl_Usuarios_Especificos>().ToList();

                // Cerrar la conexión
                factory.SQL.Close();

                // Retornar la tupla con ambos sets
                return view;

            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Notificacion_Usuarios_Solicitudes_View> GuardarNotificacionSolicitud(string? folio, string? mensaje, int idreferencia, string? usuario, string? usuarioNotificar)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    mensaje = mensaje,
                    idreferencia = idreferencia,
                    usuario = usuario,
                    usuarioNotificar = usuarioNotificar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var result = await factory.SQL.QueryMultipleAsync(", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var result = await factory.SQL.QueryMultipleAsync("HumayaDigital_Eventos.dbo.Obtener_Mensaje_Push_Especifico_Solicitud", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Notificacion_Usuarios_Solicitudes_View view = new mdl_Notificacion_Usuarios_Solicitudes_View();
                view.notificacionCuerpo = result.Read<mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo>().FirstOrDefault();
                view.notificacionUsuarios = result.Read<mdl_Usuarios_Especificos>().ToList();

                // Cerrar la conexión
                factory.SQL.Close();

                // Retornar la tupla con ambos sets
                return view;

            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
