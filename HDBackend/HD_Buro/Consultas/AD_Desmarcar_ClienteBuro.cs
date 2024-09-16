using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Desmarcar_ClienteBuro
    {
        private string CadenaConexion;
        public AD_Desmarcar_ClienteBuro(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Desmarcar_ClienteBuro>> cliente(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Desmarcar_ClienteBuro> result = await factory.SQL.QueryAsync<mdl_Desmarcar_ClienteBuro>("Credito.Elimina_Comentario_ClienteBuro", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
