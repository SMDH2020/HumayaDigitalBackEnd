using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Obtener
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Obtener(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_Solicitud?> Obtener(long solicitudId)
        {
            try
            {
                var parametros = new { solicitudId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_TI_Solicitud>(
                    "dbo.sp_TI_Solicitudes_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
