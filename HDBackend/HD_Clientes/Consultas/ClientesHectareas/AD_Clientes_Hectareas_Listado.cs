using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes_Hectareas
{
    public class AD_Clientes_Hectareas_Listado
    {
        private string CadenaConexion;
        public AD_Clientes_Hectareas_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Hectareas>> Listado(short filtrar)
        {
            try
            {
                var parametros = new
                {
                    filtrar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_Hectareas> result = await factory.SQL.QueryAsync<mdlClientes_Hectareas>("Credito.sp_clientes_hectareas_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
