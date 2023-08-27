using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_BuscarRFCOrRazonSocial
    {
        private string CadenaConexion;
        public AD_Clientes_BuscarRFCOrRazonSocial(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes>> Listado(string value)
        {
            try
            {
                var parametros = new
                {
                    value
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes> result = await factory.SQL.QueryAsync<mdlClientes>("Credito.Sp_Clientes_BuscarRFZORazonSocial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
