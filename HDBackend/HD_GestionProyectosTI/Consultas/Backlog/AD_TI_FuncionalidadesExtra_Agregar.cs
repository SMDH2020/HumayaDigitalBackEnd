using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Backlog
{
    public class AD_TI_FuncionalidadesExtra_Agregar
    {
        private string CadenaConexion;
        public AD_TI_FuncionalidadesExtra_Agregar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<(long FuncionalidadId, long SolicitudId)> Agregar(mdl_TI_FuncionalidadExtraCrear mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.AreaSolicitante,
                    mdl.Anio,
                    mdl.Nombre,
                    mdl.Descripcion,
                    mdl.DiasEstimados,
                    mdl.MesProgramado,
                    mdl.AnioProgramado,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstAsync<(long FuncionalidadId, long SolicitudId)>(
                    "dbo.sp_TI_FuncionalidadesExtra_Agregar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
