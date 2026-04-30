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
    public class AD_Obtener_Chat_Mensajes
    {
        private string CadenaConexion;
        public AD_Obtener_Chat_Mensajes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Obtener_Chat_Mensajes>> obtenerChat(string numeroTelefono)
        {
            try
            {
                var parametros = new
                {
                    numeroTelefono = numeroTelefono
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Chat_Mensajes> result = await factory.SQL.QueryAsync<mdl_Obtener_Chat_Mensajes>("HD_Mensajeria.dbo.sp_Obtener_Chat_Mensajes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Get_Rol_Mensajeria> GetRolMensajeria(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Get_Rol_Mensajeria>("HD_Mensajeria.dbo.sp_get_rol_mensajeria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
