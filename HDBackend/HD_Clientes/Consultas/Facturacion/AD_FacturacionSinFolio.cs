using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturacion;

namespace HD.Clientes.Consultas.Facturacion
{
    public class AD_FacturacionSinFolio
    {
        private string CadenaConexion;
        public AD_FacturacionSinFolio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlFAC_FacturacionEquipo>> ObtenerFacturasSinFolio(string filtro, string sucursal)
        {
            try
            {
                string queryString = "Credito.sp_Obtener_Facturas_Sin_Folio";
                FactoryConection conexion = new FactoryConection(CadenaConexion);

                var prm = new
                {
                    filtro,
                    sucursal
                };

                var result = await conexion.SQL.QueryAsync<mdlFAC_FacturacionEquipo>(queryString, prm, commandType: System.Data.CommandType.StoredProcedure);
                conexion.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
