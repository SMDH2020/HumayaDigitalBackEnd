using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_ResumenSeguimiento
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_ResumenSeguimiento(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_ResumenSeguimiento_View?> Obtener(long solicitudId)
        {
            try
            {
                var parametros = new { SolicitudId = solicitudId };
                FactoryConection factory = new FactoryConection(CadenaConexion);

                using var multi = await factory.SQL.QueryMultipleAsync(
                    "dbo.sp_TI_Solicitudes_ResumenSeguimiento", parametros, commandType: System.Data.CommandType.StoredProcedure);

                var resumen = await multi.ReadFirstOrDefaultAsync<mdl_TI_ResumenSeguimiento_View>();
                if (resumen != null)
                {
                    resumen.AprobacionesAlcance = (await multi.ReadAsync<mdl_TI_AprobacionResumen>()).ToList();
                    resumen.AprobacionesPrioridad = (await multi.ReadAsync<mdl_TI_AprobacionResumen>()).ToList();
                    resumen.AprobacionesRepriorizacion = (await multi.ReadAsync<mdl_TI_AprobacionResumen>()).ToList();
                }

                factory.SQL.Close();
                return resumen;
            }
            catch (System.Data.SqlClient.SqlException ex) when (ex.Number >= 50000)
            {
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
