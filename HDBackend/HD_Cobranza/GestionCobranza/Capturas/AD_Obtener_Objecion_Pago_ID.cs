using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Obtener_Objecion_Pago_ID
    {
        private string CadenaConexion;
        public AD_Obtener_Objecion_Pago_ID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Objecion_Pago>> Objecion(int id_Objecion)
        {
            try
            {
                var parametros = new
                {
                    @id_Objecion = id_Objecion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Objecion_Pago> result = await factory.SQL.QueryAsync<mdl_Objecion_Pago>("GestionCobranza.sp_Obtener_Objeciones_Pago_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
