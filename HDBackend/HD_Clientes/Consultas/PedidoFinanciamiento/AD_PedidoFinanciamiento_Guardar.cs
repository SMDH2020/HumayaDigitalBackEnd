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
                    inicio_interes = mdl.inicio_interes,
                    importefinanciar = mdl.importefinanciar,
                    dias = mdl.dias,
                    tasa = mdl.tasa,
                    interes = mdl.interes,
                    totalpagar = mdl.totalpagar,
                    tipo_amortizacion = mdl.tipo_amortizacion,
                    valor_insoluto = mdl.valor_insoluto,
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

        public async Task<mdlPedido_Detalle_Financiamiento_View> CrearTablaAmortizacion(mdlTabla_Amortizaciones mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    inicio = mdl.inicio,
                    valor_total = mdl.valor_total,
                    importe = mdl.importe,
                    plazo = mdl.plazo,
                    tasa = mdl.tasa,
                    usuario = mdl.usuario,

                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.Pedido_Detalle_Financiamiento_CrearTablaAmortizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPedido_Detalle_Financiamiento_View view = new mdlPedido_Detalle_Financiamiento_View();
                view.info = result.Read<mdlPedido_Detalle_Financiamiento_Info>().FirstOrDefault();
                view.detalle_financiamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
