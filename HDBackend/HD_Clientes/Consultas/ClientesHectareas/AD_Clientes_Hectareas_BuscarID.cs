using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes_Hectareas
{
    public class AD_Clientes_Hectareas_BuscarID
    {
        private string CadenaConexion;
        public AD_Clientes_Hectareas_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Hectareas> BuscarID(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };
                mdlClientes_Hectareas result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Hectareas>("Credito.sp_clientes_hectareas_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
