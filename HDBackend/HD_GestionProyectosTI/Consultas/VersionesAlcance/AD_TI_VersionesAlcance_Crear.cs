using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.VersionesAlcance
{
    public class AD_TI_VersionesAlcance_Crear
    {
        private string CadenaConexion;
        public AD_TI_VersionesAlcance_Crear(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long VersionAlcanceId, int NumeroVersion)> Crear(mdl_TI_VersionAlcanceCrear mdl)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.Objetivo,
                    mdl.AlcanceIncluido,
                    mdl.AlcanceExcluido,
                    mdl.Supuestos,
                    mdl.Dependencias,
                    mdl.UsuarioId,
                    json = JsonConvert.SerializeObject(mdl.Funcionalidades)
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long VersionAlcanceId, int NumeroVersion)>(
                    "dbo.sp_TI_VersionesAlcance_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
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
