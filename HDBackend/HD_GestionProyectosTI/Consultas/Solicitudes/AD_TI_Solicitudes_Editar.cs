using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Solicitudes
{
    public class AD_TI_Solicitudes_Editar
    {
        private string CadenaConexion;
        public AD_TI_Solicitudes_Editar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task Editar(mdl_TI_SolicitudEditar mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.SolicitudId,
                    mdl.Titulo,
                    mdl.DescripcionNecesidad,
                    mdl.ProblemaActual,
                    mdl.BeneficioEsperado,
                    mdl.TipoSolicitudId,
                    mdl.AreaSolicitante,
                    UsuarioId = usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_Solicitudes_Editar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
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
