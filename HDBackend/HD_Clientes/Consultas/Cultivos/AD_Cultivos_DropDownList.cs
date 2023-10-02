using Dapper;
using HD.AccesoDatos;

namespace HD.Clientes.Consultas.Cultivos
{
    public class AD_Cultivos_DropDownList
    {
        private string CadenaConexion;
        public AD_Cultivos_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDropDownList>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idgiroempresarial = 1
                };
                IEnumerable<mdlDropDownList> result = await factory.SQL.QueryAsync<mdlDropDownList>("Credito.sp_Cultivos_DropDownList",parametros, commandType: System.Data.CommandType.StoredProcedure);
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
