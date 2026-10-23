using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Catalogos
{
    public class AD_TI_Catalogos_CriteriosPrioridad_Listado
    {
        private string CadenaConexion;
        public AD_TI_Catalogos_CriteriosPrioridad_Listado(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_CriterioPrioridad>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_CriterioPrioridad> result = await factory.SQL.QueryAsync<mdl_TI_CriterioPrioridad>(
                    "dbo.sp_TI_Catalogos_CriteriosPrioridad_Listado", commandType: System.Data.CommandType.StoredProcedure);
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
