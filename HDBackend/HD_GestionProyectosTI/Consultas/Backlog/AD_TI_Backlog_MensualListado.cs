using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Backlog
{
    public class AD_TI_Backlog_MensualListado
    {
        private string CadenaConexion;
        public AD_TI_Backlog_MensualListado(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_BacklogFuncionalidad>> Listado(byte mes, short anio)
        {
            try
            {
                var parametros = new { Mes = mes, Anio = anio };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_BacklogFuncionalidad> result = await factory.SQL.QueryAsync<mdl_TI_BacklogFuncionalidad>(
                    "dbo.sp_TI_Backlog_Mensual_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
