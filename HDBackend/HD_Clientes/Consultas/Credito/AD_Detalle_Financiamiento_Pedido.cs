using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Credito;

namespace HD.Clientes.Consultas.Credito
{
    public class AD_Detalle_Financiamiento_Pedido
    {
        private string CadenaConexion;
        public AD_Detalle_Financiamiento_Pedido(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Detalle_Financiamiento_Pedido>> detalle(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @folio = folio
                };
                IEnumerable<mdl_Detalle_Financiamiento_Pedido> result = await factory.SQL.QueryAsync<mdl_Detalle_Financiamiento_Pedido>("Credito.sp_Detalle_Financiamiento_Pedido_PorFolio", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
