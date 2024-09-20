using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Credito;

namespace HD.Clientes.Consultas.Credito
{
    public class AD_Operaciones_noEncontradas_Equip
    {
        private string CadenaConexion;
        public AD_Operaciones_noEncontradas_Equip(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Operaciones_noEncontradas_Equip>> operaciones()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Operaciones_noEncontradas_Equip> result = await factory.SQL.QueryAsync<mdl_Operaciones_noEncontradas_Equip>("Credito.sp_Listado_Operaciones_noEncontradas_Equip", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
