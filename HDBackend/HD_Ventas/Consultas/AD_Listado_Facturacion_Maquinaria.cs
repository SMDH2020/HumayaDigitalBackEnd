using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Listado_Facturacion_Maquinaria
    {
        private string CadenaConexion;
        public AD_Listado_Facturacion_Maquinaria(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListado_Facturacion_Maquinaria>> Get(int ejercicio, int periodo, string adr, string sucursal, int linea)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursales = sucursal,
                    linea = linea
                };
                IEnumerable<mdlListado_Facturacion_Maquinaria> result = await factory.SQL.QueryAsync<mdlListado_Facturacion_Maquinaria>("Ventas.sp_Listado_Facturacion_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
