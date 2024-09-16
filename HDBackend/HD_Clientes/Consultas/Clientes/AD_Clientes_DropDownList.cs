using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System.Security.Cryptography;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_DropDownList
    {
        private string CadenaConexion;
        public AD_Clientes_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDropDownList>> DropDownList(string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario = usuario
                };
                IEnumerable<mdlDropDownList> result = await factory.SQL.QueryAsync<mdlDropDownList>("Credito.sp_clientes_dropdownlist",parametros, commandType: System.Data.CommandType.StoredProcedure);
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
