using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class AD_Eliminar_Facturacion_Maquinaria
    {
        private string CadenaConexion;
        public AD_Eliminar_Facturacion_Maquinaria(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDetalle_Facturacion_Maquinaria>> Eliminar(int id, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idfactura = id,
                    usuario = usuario
                };
                IEnumerable<mdlDetalle_Facturacion_Maquinaria> result = await factory.SQL.QueryAsync<mdlDetalle_Facturacion_Maquinaria>("Ventas.sp_Eliminar_Facturacion_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
