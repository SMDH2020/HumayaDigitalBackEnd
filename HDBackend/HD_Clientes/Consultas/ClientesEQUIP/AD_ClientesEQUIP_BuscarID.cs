using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;


namespace HD.Clientes.Consultas.ClientesEQUIP
{
    public class AD_ClientesEQUIP_BuscarID
    {
        private string CadenaConexion;
        public AD_ClientesEQUIP_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_EQUIP> BuscarID(int idcliente_equip)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente_equip
                };
                mdlClientes_EQUIP result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_EQUIP>("Credito.sp_clientes_equip_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
