using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Catalogos
{
    public class AD_TI_Catalogos_TiposSolicitud_Listado
    {
        private string CadenaConexion;
        public AD_TI_Catalogos_TiposSolicitud_Listado(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_TipoSolicitud>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_TipoSolicitud> result = await factory.SQL.QueryAsync<mdl_TI_TipoSolicitud>(
                    "dbo.sp_TI_Catalogos_TiposSolicitud_Listado", commandType: System.Data.CommandType.StoredProcedure);
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
