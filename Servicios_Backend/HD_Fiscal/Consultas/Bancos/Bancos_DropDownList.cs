using Dapper;
using HD_AccesoDatos;
using HD_Generales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HD_Fiscal.Consultas.Bancos
{
    public class Bancos_DropDownList
    {
        FactoryConnection factory;
        public string Mensaje { get; private set; }
        public Bancos_DropDownList(string CadenaConexion)
        {
            factory = new FactoryConnection(CadenaConexion);
        }
        public async Task<IEnumerable<mdlDropDownList>> Listado()
        {
            try
            {
                var result = await factory.SQL.QueryAsync<mdlDropDownList>("sp_bancos_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                Mensaje = ex.Message;
                return null;
            }
        }
    }
}
