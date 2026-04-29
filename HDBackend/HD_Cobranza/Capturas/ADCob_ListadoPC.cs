using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class ADCob_ListadoPC
    {
        private string CadenaConexion;
        public ADCob_ListadoPC(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListadoPC>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result =await factory.SQL.QueryAsync<mdlListadoPC>("Cobranza.sp_Obtener_Listado_PC", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
