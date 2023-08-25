using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_DropDownList
    {
        private string CadenaConexion;
        public AD_Clientes_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes> result = await factory.SQL.QueryAsync<mdlClientes>("sp_clientes_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
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
