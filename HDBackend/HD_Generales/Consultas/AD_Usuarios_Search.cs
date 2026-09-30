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
    public class AD_Usuarios_Search
    {
        private string CadenaConexion;
        public AD_Usuarios_Search(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlUsuariosSearch>> Listado(int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idusuario = idusuario
                };

                var result = await factory.SQL.QueryAsync<mdlUsuariosSearch>("dbo.Sp_Usuarios_Search", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
