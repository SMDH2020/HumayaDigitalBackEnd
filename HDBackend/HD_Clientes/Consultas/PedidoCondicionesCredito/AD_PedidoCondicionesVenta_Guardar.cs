using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoCondicionesCredito
{
    public class AD_PedidoCondicionesVenta_Guardar
    {
        private string CadenaConexion;
        public AD_PedidoCondicionesVenta_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlPedido_Condiciones_Venta mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    condiciones = mdl.condiciones,
                    observaciones = mdl.observaciones,
                    deposito = mdl.deposito,
                    taza = mdl.taza,
                    anticipo = mdl.anticipo,
                    plazo = mdl.plazo,
                    tiempo_plazo=mdl.tiempo_plazo,
                    mhusajdf = mdl.mhusajdf,
                    gastos = mdl.gastos,
                    enganche = mdl.enganche,
                    moneda = mdl.moneda,
                    usuario = mdl.usuario
                };
                 await factory.SQL.QueryAsync("Credito.sp_Pedido_Condiciones_Venta_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
