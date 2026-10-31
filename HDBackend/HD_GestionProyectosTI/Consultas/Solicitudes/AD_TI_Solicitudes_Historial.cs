using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Historial
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Historial(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_HistorialEstado>> Historial(long solicitudId)
        {
            try
            {
                var parametros = new { solicitudId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_HistorialEstado> result = await factory.SQL.QueryAsync<mdl_TI_HistorialEstado>(
                    "dbo.sp_TI_Solicitudes_Historial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
