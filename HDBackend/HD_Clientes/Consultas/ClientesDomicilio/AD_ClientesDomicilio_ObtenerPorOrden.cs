using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDomicilio
{
    public class AD_ClientesDomicilio_ObtenerPorOrden
    {
        private string CadenaConexion;
        public AD_ClientesDomicilio_ObtenerPorOrden(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientesDomicilioList> Get(int idcliente,int orden)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    orden
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdlClientesDomicilioList result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientesDomicilioList>("Credito.sp_Clientes_Domicilios_ObtenerPorOrden", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
