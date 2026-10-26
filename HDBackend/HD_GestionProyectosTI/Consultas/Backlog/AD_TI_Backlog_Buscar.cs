using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Backlog
{
    public class AD_TI_Backlog_Buscar
    {
        private string CadenaConexion;
        public AD_TI_Backlog_Buscar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_BacklogFuncionalidad>> Buscar(byte mes, short anio, string? busqueda)
        {
            try
            {
                var parametros = new { Mes = mes, Anio = anio, Busqueda = busqueda };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_BacklogFuncionalidad> result = await factory.SQL.QueryAsync<mdl_TI_BacklogFuncionalidad>(
                    "dbo.sp_TI_Backlog_Buscar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
