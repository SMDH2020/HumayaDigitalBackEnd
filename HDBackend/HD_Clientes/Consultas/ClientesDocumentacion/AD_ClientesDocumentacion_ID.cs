using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDocumentacion
{
    public class AD_ClientesDocumentacion_ID
    {
        private string CadenaConexion;
        public AD_ClientesDocumentacion_ID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientesDocumentacion_BuscarID> BuscarID(int idclientedocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idclientedocumento
                };
                mdlClientesDocumentacion_BuscarID result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientesDocumentacion_BuscarID>("Credito.sp_Clientes_Documentacion_BuscarPorID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
