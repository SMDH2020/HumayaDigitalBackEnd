using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Credito;

namespace HD.Clientes.Consultas.Credito
{
    public class AD_Facturas_Diferencias_Vencimiento
    {
        private string CadenaConexion;
        public AD_Facturas_Diferencias_Vencimiento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Facturas_Diferencia_Vencimiento>> facturas()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                IEnumerable<mdl_Facturas_Diferencia_Vencimiento> result = await factory.SQL.QueryAsync<mdl_Facturas_Diferencia_Vencimiento>("Credito.sp_Facturas_Diferencias_Fechas_Vencimiento_HD_EQUIP", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
