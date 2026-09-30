using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Responsables_Cobranza_DropDownList
    {
        private string CadenaConexion;
        public AD_Responsables_Cobranza_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Responsables_Cobranza_DropDownList>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Responsables_Cobranza_DropDownList> result = await factory.SQL.QueryAsync<mdl_Responsables_Cobranza_DropDownList>("GestionCobranza.sp_Responsables_Cobranza_DropDownList", commandType: System.Data.CommandType.StoredProcedure);
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
