using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.VersionesAlcance
{
    public class AD_TI_VersionesAlcance_ListadoPorSolicitud
    {
        private string CadenaConexion;
        public AD_TI_VersionesAlcance_ListadoPorSolicitud(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_VersionAlcance>> Listado(long solicitudId)
        {
            try
            {
                var parametros = new { solicitudId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_TI_VersionAlcance>(
                    "dbo.sp_TI_VersionesAlcance_ListadoPorSolicitud", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
