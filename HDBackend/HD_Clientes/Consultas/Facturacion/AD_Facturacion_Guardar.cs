using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturacion;

namespace HD.Clientes.Consultas.Facturacion
{
    public class AD_Facturacion_Guardar
    {
        private string CadenaConexion;
        public AD_Facturacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<DResult> Guardar(mdlFAC_FacturasByFolio vm)
        {
            try
            {
                FactoryConection conexion = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idfactura = vm.idfactura,
                    serie = vm.serie,
                    folio = vm.folio,
                    cliente = vm.cliente,
                    documento = vm.documento,
                    fechasuscripcion = vm.fechasuscripcion,
                    tieneinteres = vm.tieneinteres,
                    tasa = vm.tasa,
                    intereses = vm.intereses,
                    usuario = vm.usuario,
                    montofinanciado = vm.montofinanciado,
                    montointereses = vm.montointereses,
                    vendedor = vm.vendedor,
                    vencimiento = vm.vencimiento,
                    financiera = vm.financiera
                };
                await conexion.SQL.QueryAsync("credito.sp_FacturacionSeriesOrigen_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                conexion.SQL.Close();
                return new DResult();
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
