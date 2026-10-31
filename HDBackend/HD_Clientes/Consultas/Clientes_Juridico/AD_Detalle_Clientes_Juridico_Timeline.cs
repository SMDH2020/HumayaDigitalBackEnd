using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Clientes_Juridico;

namespace HD.Clientes.Consultas.Clientes_Juridico
{
    public class AD_Detalle_Clientes_Juridico_Timeline
    {
        private string CadenaConexion;
        public AD_Detalle_Clientes_Juridico_Timeline(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Timeline_Cliente_Judicial>> TimelineCliente(int idregistro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idregistro = idregistro
                };
                IEnumerable<mdl_Timeline_Cliente_Judicial> result = await factory.SQL.QueryAsync<mdl_Timeline_Cliente_Judicial>("Credito.sp_Obtener_Timelinea_Cliente_Judicial", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Timeline_Demanda_Judicial>> TimelineDemanda(int idregistro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idregistro = idregistro
                };
                IEnumerable<mdl_Timeline_Demanda_Judicial> result = await factory.SQL.QueryAsync<mdl_Timeline_Demanda_Judicial>("Credito.sp_Obtener_Timelinea_Demanda_Judicial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
