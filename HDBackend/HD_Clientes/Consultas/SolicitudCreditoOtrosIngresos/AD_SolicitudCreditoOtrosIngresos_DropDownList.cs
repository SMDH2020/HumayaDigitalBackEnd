using Dapper;
using HD.AccesoDatos;

namespace HD.Clientes.Consultas.SolicitudCreditoOtrosIngresos
{
    public class AD_SolicitudCreditoOtrosIngresos_DropDownList
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoOtrosIngresos_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDropDownList>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDropDownList> result = await factory.SQL.QueryAsync<mdlDropDownList>("sp_solicitud_credito_otros_ingresos_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
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
