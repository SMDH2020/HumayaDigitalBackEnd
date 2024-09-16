using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Control_Clientes;

namespace HD.Clientes.Consultas.Control_Clientes
{
    public class ADCC_Clientes_Search
    {
        private string CadenaConexion;
        public ADCC_Clientes_Search(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCC_Clientes_search>> Guardar(string razon_social)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @razon_social=razon_social
                };

                var result = await factory.SQL.QueryAsync<mdlCC_Clientes_search>("Credito.sp_Clientes_Search", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
