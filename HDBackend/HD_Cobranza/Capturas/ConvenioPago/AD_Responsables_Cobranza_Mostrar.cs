using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class AD_Responsables_Cobranza_Mostrar
    {
        private string CadenaConexion;
        public AD_Responsables_Cobranza_Mostrar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<MdlResponsables_Cobranza_Mostrar>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<MdlResponsables_Cobranza_Mostrar> result = await factory.SQL.QueryAsync<MdlResponsables_Cobranza_Mostrar>("Credito.sp_Responsables_Cobranza_Mostrar", commandType: System.Data.CommandType.StoredProcedure);
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
