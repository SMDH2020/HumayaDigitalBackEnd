using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;

namespace HD.Clientes.Consultas.SolicitudCredito_Analisis
{
    public class AD_SCAnalisis_Tablero
    {
        private string CadenaConexion;
        public AD_SCAnalisis_Tablero(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSCAnalisis_Tablero>> Get(string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario
                };
                IEnumerable<mdlSCAnalisis_Tablero> result = await factory.SQL.QueryAsync<mdlSCAnalisis_Tablero>("Credito.sp_Solicitud_Credito_Tablas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
