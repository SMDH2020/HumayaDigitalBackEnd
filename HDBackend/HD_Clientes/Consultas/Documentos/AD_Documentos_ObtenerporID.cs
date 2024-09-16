using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Documentos
{
    public class AD_Documentos_ObtenerporID
    {
        private string CadenaConexion;
        public AD_Documentos_ObtenerporID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlDocumentos> BuscarID(int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    iddocumento
                };
                mdlDocumentos result = await factory.SQL.QueryFirstOrDefaultAsync<mdlDocumentos>("Credito.sp_Documentos_ObtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
