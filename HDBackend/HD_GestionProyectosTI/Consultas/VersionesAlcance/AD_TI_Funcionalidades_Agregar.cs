using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;
using System.Data.SqlClient;

namespace HD_GestionProyectosTI.Consultas.VersionesAlcance
{
    public class AD_TI_Funcionalidades_Agregar
    {
        private string CadenaConexion;
        public AD_TI_Funcionalidades_Agregar(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<long> Agregar(mdl_TI_FuncionalidadAgregar mdl, int usuarioId)
        {
            try
            {
                var parametros = new
                {
                    mdl.VersionAlcanceId,
                    mdl.Codigo,
                    mdl.Nombre,
                    mdl.Descripcion,
                    mdl.ReglasNegocio,
                    mdl.CriteriosAceptacion,
                    mdl.PrioridadInterna,
                    mdl.Complejidad,
                    mdl.Orden,
                    mdl.DiasEstimados,
                    usuarioId
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                long funcionalidadId = await factory.SQL.QueryFirstAsync<long>(
                    "dbo.sp_TI_Funcionalidades_Agregar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return funcionalidadId;
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
