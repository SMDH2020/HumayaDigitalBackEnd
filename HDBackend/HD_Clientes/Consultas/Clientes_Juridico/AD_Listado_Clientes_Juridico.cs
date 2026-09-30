using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Clientes_Juridico;

namespace HD.Clientes.Consultas.Clientes_Juridico
{
    public class AD_Listado_Clientes_Juridico
    {
        private string CadenaConexion;
        public AD_Listado_Clientes_Juridico(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Clientes_Juridico>> clientes()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                IEnumerable<mdl_Clientes_Juridico> result = await factory.SQL.QueryAsync<mdl_Clientes_Juridico>("Credito.sp_Listado_Clientes_Judicial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
