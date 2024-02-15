using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class ADAnalisis_Pedido_Estado
    {
        private string CadenaConexion;
        public ADAnalisis_Pedido_Estado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Pedido_Estado> Get(string folio,string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                mdlSCAnalisis_Pedido_Estado result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSCAnalisis_Pedido_Estado>("Credito.sp_Analisis_Pedido_Estado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
