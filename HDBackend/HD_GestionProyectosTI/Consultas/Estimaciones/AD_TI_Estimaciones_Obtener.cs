using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Estimaciones
{
    public class AD_TI_Estimaciones_Obtener
    {
        private string CadenaConexion;
        public AD_TI_Estimaciones_Obtener(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_Estimacion?> Obtener(long estimacionId)
        {
            try
            {
                var parametros = new { estimacionId };
                FactoryConection factory = new FactoryConection(CadenaConexion);

                using var multi = await factory.SQL.QueryMultipleAsync(
                    "dbo.sp_TI_Estimaciones_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);

                var estimacion = await multi.ReadFirstOrDefaultAsync<mdl_TI_Estimacion>();
                if (estimacion != null)
                    estimacion.Detalle = (await multi.ReadAsync<mdl_TI_EstimacionDetalleItem>()).ToList();

                factory.SQL.Close();
                return estimacion;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
