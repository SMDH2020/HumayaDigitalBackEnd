using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Consultas
{
    public class AD_Notificaciones_Whatsapp_Borrar_Notificacion
    {
        private string CadenaConexion;
        public AD_Notificaciones_Whatsapp_Borrar_Notificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> Borrar(int idnotificacion, string usuario)
        {
            try
            {
                var parametros = new
                {
                    @idnotificacion = idnotificacion,
                    @usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("HD_Mensajeria.dbo.sp_Notificaciones_Whatsapp_Borrar_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> BorrarTodo(string usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("HD_Mensajeria.dbo.sp_Notificaciones_Whatsapp_Borrar_Notificacion_Total", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
