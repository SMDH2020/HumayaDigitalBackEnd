using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesEQUIP
{
    public class AD_ClientesEQUIP_Listado
    {
        private string CadenaConexion;
        public AD_ClientesEQUIP_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_EQUIP>> Listado(short filtrar)
        {
            try
            {
                var parametros = new
                {
                    filtrar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_EQUIP> result = await factory.SQL.QueryAsync<mdlClientes_EQUIP>("Credito.sp_clientes_equip_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
