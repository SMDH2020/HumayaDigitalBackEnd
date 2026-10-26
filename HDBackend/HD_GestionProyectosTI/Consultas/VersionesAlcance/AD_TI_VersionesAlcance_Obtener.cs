using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.VersionesAlcance
{
    public class AD_TI_VersionesAlcance_Obtener
    {
        private string CadenaConexion;
        public AD_TI_VersionesAlcance_Obtener(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<mdl_TI_VersionAlcance?> Obtener(long versionAlcanceId)
        {
            try
            {
                var parametros = new { versionAlcanceId };
                FactoryConection factory = new FactoryConection(CadenaConexion);

                using var multi = await factory.SQL.QueryMultipleAsync(
                    "dbo.sp_TI_VersionesAlcance_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);

                var version = await multi.ReadFirstOrDefaultAsync<mdl_TI_VersionAlcance>();
                if (version != null)
                    version.Funcionalidades = (await multi.ReadAsync<mdl_TI_Funcionalidad>()).ToList();

                factory.SQL.Close();
                return version;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
