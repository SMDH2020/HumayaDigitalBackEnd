using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.OrdenesAbiertas;

namespace Postventa.Consultas.OrdenesAbiertas
{
    public class AD_OT_Comentario_Listado
    {
        private string CadenaConexion;
        public AD_OT_Comentario_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_OT_Comentario_Item>> Listado(int ordenTrabajoId)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("OrdenTrabajoId", ordenTrabajoId, System.Data.DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_OT_Comentario_Item> result = await factory.SQL.QueryAsync<mdl_OT_Comentario_Item>(
                    "Postventa.sp_OT_Comentario_Listado",
                    parametros,
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
