using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.OrdenesAbiertas;

namespace Postventa.Consultas.OrdenesAbiertas
{
    public class AD_OT_Comentario_Guardar
    {
        private string CadenaConexion;
        public AD_OT_Comentario_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        // El SP solo inserta y devuelve el NewId (SCOPE_IDENTITY); se vuelve a
        // consultar el listado para regresar el historial actualizado al frontend.
        public async Task<IEnumerable<mdl_OT_Comentario_Item>> Guardar(mdl_OT_Comentario_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("OrdenTrabajoId", mdl.OrdenTrabajoId, System.Data.DbType.Int32);
                parametros.Add("CategoriaId", mdl.CategoriaId, System.Data.DbType.Int32);
                parametros.Add("Comentario", mdl.Comentario, System.Data.DbType.String);
                parametros.Add("FechaEstimadaCierre", mdl.FechaEstimadaCierre, System.Data.DbType.DateTime);
                parametros.Add("UsuarioRegistro", mdl.UsuarioRegistro, System.Data.DbType.String);

                await factory.SQL.ExecuteAsync(
                    "Postventa.sp_OT_Comentario_Guardar",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                AD_OT_Comentario_Listado listado = new AD_OT_Comentario_Listado(CadenaConexion);
                return await listado.Listado(mdl.OrdenTrabajoId);
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
