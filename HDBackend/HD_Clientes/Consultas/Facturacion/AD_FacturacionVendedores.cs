using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturacion;

namespace HD.Clientes.Consultas.Facturacion
{
    public class AD_FacturacionVendedores
    {
        private string CadenaConexion;
        public AD_FacturacionVendedores(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlFAC_DatosVendedor>> ObtenerVendedores()
        {
            try
            {
                var stringquery = "select id as idvendedor,nombre from EQUIP.dbo.Vendedores where estatus=1";
                FactoryConection conexion = new FactoryConection(CadenaConexion);

                var result = await conexion.SQL.QueryAsync<mdlFAC_DatosVendedor>(stringquery, commandType: System.Data.CommandType.Text);
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
