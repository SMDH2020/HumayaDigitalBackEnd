using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoFinanciamiento
{
    public class AD_PedidoFinanciamiento_Guardar
    {
        private string CadenaConexion;
        public AD_PedidoFinanciamiento_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlPedido_Detalle_Financiamiento mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    docto = mdl.docto,
                    vencimiento = mdl.vencimiento,
                    importefinanciar=mdl.importefinanciar,
                    dias = mdl.dias,
                    tasa = mdl.tasa,
                    interes = mdl.interes,
                    totalpagar = mdl.totalpagar,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.Pedido_Detalle_Financiamiento_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
