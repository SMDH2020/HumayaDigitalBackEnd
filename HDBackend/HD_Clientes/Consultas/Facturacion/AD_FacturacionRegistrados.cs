using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturacion;

namespace HD.Clientes.Consultas.Facturacion
{
    public class AD_FacturacionRegistrados
    {
        private string CadenaConexion;
        public AD_FacturacionRegistrados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlFAC_FacturasByFolio> ObtenerFacturas(string id)
        {
            try
            {
                string queryString = "Credito.sp_FacturacionSeriesOrigen_ObtenerById";
                FactoryConection conexion = new FactoryConection(CadenaConexion);

                var prm = new
                {
                    id
                };

                var result = await conexion.SQL.QueryFirstOrDefaultAsync<mdlFAC_FacturasByFolio>(queryString, prm, commandType: System.Data.CommandType.StoredProcedure);
                conexion.SQL.Close();
                if (result == null) { result = new mdlFAC_FacturasByFolio(); }
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
