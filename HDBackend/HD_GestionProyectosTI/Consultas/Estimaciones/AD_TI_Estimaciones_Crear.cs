using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Estimaciones
{
    public class AD_TI_Estimaciones_Crear
    {
        private string CadenaConexion;
        public AD_TI_Estimaciones_Crear(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long EstimacionId, int NumeroVersion)> Crear(mdl_TI_EstimacionCrear mdl)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.VersionAlcanceId,
                    mdl.RegistradaPorUsuarioId,
                    mdl.Riesgos,
                    mdl.Supuestos,
                    mdl.Dependencias,
                    mdl.DiasCalendarioEstimados,
                    json = JsonConvert.SerializeObject(mdl.Detalle)
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long EstimacionId, int NumeroVersion)>(
                    "dbo.sp_TI_Estimaciones_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
