using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Consultas
{
    public class AD_Apartar_Chat
    {
        private string CadenaConexion;
        public AD_Apartar_Chat(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> Guardar(string numeroTelefono, string usuario)
        {
            try
            {
                var parametros = new
                {
                    numeroTelefono = numeroTelefono,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("HD_Mensajeria.dbo.sp_Apartar_Chat_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> Eliminar(string numeroTelefono, string usuario)
        {
            try
            {
                var parametros = new
                {
                    numeroTelefono = numeroTelefono,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("HD_Mensajeria.dbo.sp_Apartar_Chat_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
