using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.OrdenesAbiertas;

namespace Postventa.Consultas.OrdenesAbiertas
{
    public class AD_OT_Comentario_Categorias_Listado
    {
        private string CadenaConexion;
        public AD_OT_Comentario_Categorias_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_OT_Comentario_Categoria>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_OT_Comentario_Categoria> result = await factory.SQL.QueryAsync<mdl_OT_Comentario_Categoria>(
                    "Postventa.sp_OT_Comentario_Categorias_Listado",
                    commandType: System.Data.CommandType.StoredProcedure);
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
