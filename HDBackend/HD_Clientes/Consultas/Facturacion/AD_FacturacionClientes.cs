using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturacion;

namespace HD.Clientes.Consultas.Facturacion
{
    public class AD_FacturacionClientes
    {
        private string CadenaConexion;
        public AD_FacturacionClientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlFAC_DatosCliente>> ObtenerClientes()
        {
            try
            {
                var stringquery = "select distinct idCliente,RazonSocial from EQUIP.Credito.Clientes " +
                                 "where not RazonSocial like '%JOHN DEERE%'";
                FactoryConection conexion = new FactoryConection(CadenaConexion);

                var result = await conexion.SQL.QueryAsync<mdlFAC_DatosCliente>(stringquery, commandType: System.Data.CommandType.Text);
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
