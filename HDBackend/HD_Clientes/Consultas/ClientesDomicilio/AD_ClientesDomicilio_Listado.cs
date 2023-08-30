using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDomicilio
{
    public class AD_ClientesDomicilio_Listado
    {
        private string CadenaConexion;
        public AD_ClientesDomicilio_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientesDomicilioList>> Listado(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientesDomicilioList> result = await factory.SQL.QueryAsync<mdlClientesDomicilioList>("Credito.sp_Clientes_Domicilios_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
