using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Carga_ClientesBuro_Comentarios
    {
        private string CadenaConexion;
        public AD_Carga_ClientesBuro_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCarga_Comentarios_ClientesBuro>> comentarios(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idCliente = idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Comentarios_ClientesBuro> result = await factory.SQL.QueryAsync<mdlCarga_Comentarios_ClientesBuro>("Credito.sp_Obtener_Comentario_ClienteBuro", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
