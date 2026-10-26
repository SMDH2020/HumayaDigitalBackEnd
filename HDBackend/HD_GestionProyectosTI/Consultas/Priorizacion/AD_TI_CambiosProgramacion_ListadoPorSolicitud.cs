using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_CambiosProgramacion_ListadoPorSolicitud
    {
        private string CadenaConexion;
        public AD_TI_CambiosProgramacion_ListadoPorSolicitud(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_CambioProgramacion>> Listado(long solicitudId)
        {
            try
            {
                var parametros = new { SolicitudId = solicitudId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_CambioProgramacion> result = await factory.SQL.QueryAsync<mdl_TI_CambioProgramacion>(
                    "dbo.sp_TI_CambiosProgramacion_ListadoPorSolicitud", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
