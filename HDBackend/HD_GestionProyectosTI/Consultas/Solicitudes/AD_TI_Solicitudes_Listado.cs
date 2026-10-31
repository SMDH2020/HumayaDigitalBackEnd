using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Listado
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Listado(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        // verTodo = true para roles con visibilidad global (JEFATURA_TI,
        // GERENCIA_PLANEACION, ADMIN_MODULO); false para que el SP filtre
        // solo lo que el usuario solicito o donde participa.
        public async Task<IEnumerable<mdl_TI_Solicitud>> Listado(int usuarioId, bool verTodo, string? estadoCodigo, string? tipoCodigo, DateTime? fechaInicio, DateTime? fechaFin, string? areas)
        {
            try
            {
                var parametros = new { usuarioId, verTodo, estadoCodigo, tipoCodigo, fechaInicio, fechaFin, areas };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_Solicitud> result = await factory.SQL.QueryAsync<mdl_TI_Solicitud>(
                    "dbo.sp_TI_Solicitudes_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
