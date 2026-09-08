using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Consultas
{
    public class AD_RelUsuarioImei_BuscarID
    {
        private string CadenaConexion;
        public AD_RelUsuarioImei_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlRelUsuarioImei> BuscarID(int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idusuario
                };
                mdlRelUsuarioImei result = await factory.SQL.QueryFirstOrDefaultAsync<mdlRelUsuarioImei>("humayadigital_usuarios.dbo.Sp_Rel_Usuarios_Imei_BuscarID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
