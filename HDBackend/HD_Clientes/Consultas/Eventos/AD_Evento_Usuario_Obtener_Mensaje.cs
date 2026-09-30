using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Eventos
{
    public class AD_Evento_Usuario_Obtener_Mensaje
    {
        private string CadenaConexion;
        public AD_Evento_Usuario_Obtener_Mensaje(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_evento_notificacion> obtenerMensaje(string usuario, int idlog)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    idlog = idlog
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_evento_notificacion result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_evento_notificacion>("dbo..sp_Evento_Usuario_Listado_Obtener_Mensaje", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
