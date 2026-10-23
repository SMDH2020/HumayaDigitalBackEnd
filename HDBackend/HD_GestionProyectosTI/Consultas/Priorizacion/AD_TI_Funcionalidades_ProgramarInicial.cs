using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.Priorizacion
{
    public class AD_TI_Funcionalidades_ProgramarInicial
    {
        private string CadenaConexion;
        public AD_TI_Funcionalidades_ProgramarInicial(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task ProgramarInicial(mdl_TI_FuncionalidadProgramarInicial mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.FuncionalidadId,
                    mdl.MesProgramado,
                    mdl.AnioProgramado,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync(
                    "dbo.sp_TI_Funcionalidades_ProgramarInicial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
