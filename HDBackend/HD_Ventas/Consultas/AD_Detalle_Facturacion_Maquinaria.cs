using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Detalle_Facturacion_Maquinaria
    {
        private string CadenaConexion;
        public AD_Detalle_Facturacion_Maquinaria(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDetalle_Facturacion_Maquinaria>> Get(string nip)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    nip = nip
                };
                IEnumerable<mdlDetalle_Facturacion_Maquinaria> result = await factory.SQL.QueryAsync<mdlDetalle_Facturacion_Maquinaria>("Ventas.sp_Obtener_Detalle_Facturacion_NIP", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
