using Dapper;
using HD.AccesoDatos;
using HD_Mensajeria.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Consultas
{
    public class AD_Notificaciones_Whatsapp_Obtener_Listado
    {
        private string CadenaConexion;
        public AD_Notificaciones_Whatsapp_Obtener_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Notificaciones_Whatsapp>> obtenerNotificaciones(string usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Notificaciones_Whatsapp> result = await factory.SQL.QueryAsync<mdl_Notificaciones_Whatsapp>("HD_Mensajeria.dbo.sp_Notificaciones_Whatsapp_Sin_Leer", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
