using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Carga_Evidencia_Modal
    {
        private string CadenaConexion;
        public AD_Carga_Evidencia_Modal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_Convenio_Guardar mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    folio = mdl.folio,
                    documento = mdl.documento,
                    extension = mdl.extension,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("GestionCobranza.sp_Guardar_Evidencia_Modal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
