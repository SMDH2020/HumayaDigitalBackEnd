using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;

namespace HD_Auditoria.Consultas.Carga_Archivos
{
    public class AD_Listado_Auditorias_Programadas
    {
        private string CadenaConexion;
        public AD_Listado_Auditorias_Programadas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Auditorias_Programadas>> Auditorias()
        {
            try
            {
                var parametros = new DynamicParameters();
                //parametros.Add("id_categoria", id, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Auditorias_Programadas> result = await factory.SQL.QueryAsync<mdl_Listado_Auditorias_Programadas>("Auditoria.sp_Obtener_Listado_Auditorias_Prog", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
