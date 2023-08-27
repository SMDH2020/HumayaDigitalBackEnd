using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesGiroEmpresarial
{
    public class AD_ClientesGiroEmpresarial_BuscarID
    {
        private string CadenaConexion;
        public AD_ClientesGiroEmpresarial_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Giro_Empresarial> BuscarID(int idcliente_giro_empresarial)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente_giro_empresarial
                };
                mdlClientes_Giro_Empresarial result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Giro_Empresarial>("Credito.sp_mdlClientes_Giro_Empresarial_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
