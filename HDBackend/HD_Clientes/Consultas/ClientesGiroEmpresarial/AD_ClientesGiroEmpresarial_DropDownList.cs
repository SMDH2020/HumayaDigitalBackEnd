using Dapper;
using HD.AccesoDatos;

namespace HD.Clientes.Consultas.ClientesGiroEmpresarial
{
    public class AD_ClientesGiroEmpresarial_DropDownList
    {
        private string CadenaConexion;
        public AD_ClientesGiroEmpresarial_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDropDownList>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDropDownList> result = await factory.SQL.QueryAsync<mdlDropDownList>("sp_clientes_giro_empresarial_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
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
