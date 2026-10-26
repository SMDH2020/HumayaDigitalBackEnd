using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Crear
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Crear(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long SolicitudId, string Folio)> Crear(mdl_TI_SolicitudCrear mdl)
        {
            try
            {
                var parametros = new
                {
                    mdl.Titulo,
                    mdl.DescripcionNecesidad,
                    mdl.ProblemaActual,
                    mdl.BeneficioEsperado,
                    mdl.TipoSolicitudId,
                    mdl.AreaSolicitante,
                    mdl.EsConfidencial,
                    mdl.SolicitanteUsuarioId,
                    mdl.CreadaPorUsuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long SolicitudId, string Folio)>(
                    "dbo.sp_TI_Solicitudes_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
