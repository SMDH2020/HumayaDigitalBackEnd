using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Eventos
{
    public class AD_Evento_Usuario_Borrar_Notificaciones
    {
        private string CadenaConexion;
        public AD_Evento_Usuario_Borrar_Notificaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_evento_notificacion>> Borrar(int idevento_usuario, string usuario)
        {
            try
            {
                var parametros = new
                {
                    idevento_usuario,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_evento_notificacion> result = await factory.SQL.QueryAsync<mdl_evento_notificacion>("dbo.sp_Evento_Usuario_Borrar_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_evento_notificacion>> BorrarTodo(string usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_evento_notificacion> result = await factory.SQL.QueryAsync<mdl_evento_notificacion>("dbo.sp_Evento_Usuario_Borrar_Notificaciones_Total", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
