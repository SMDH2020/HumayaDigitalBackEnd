using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;

namespace HD.Clientes.Consultas.CRM
{
    public class AD_Datos_CRM
    {
        private string CadenaConexion;
        public AD_Datos_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Clientes_CRM>> Listado()
        {
            try
            {
                var parametros = new
                {
                    
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_CRM> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_CRM>("Credito.sp_Get_Clientes_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
