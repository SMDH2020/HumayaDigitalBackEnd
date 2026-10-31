using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Listado_Lineas_Venta 
    {
        private string CadenaConexion;
        public AD_Listado_Lineas_Venta(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlListadoLineasVentas>> Listado()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListadoLineasVentas> result = await factory.SQL.QueryAsync<mdlListadoLineasVentas>("Ventas.sp_Obtener_Listado_Lineas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlListadoLineasVentas>> ObtenerLineaID(int idlinea)
        {
            try
            {
                var parametros = new
                {
                    @idlinea = idlinea
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListadoLineasVentas> result = await factory.SQL.QueryAsync<mdlListadoLineasVentas>("Ventas.sp_Obtener_Linea_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Carrusel_Imagenes>> Carrusel(int idmodelo)
        {
            try
            {
                var parametros = new
                {
                    @idmodelo = idmodelo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Carrusel_Imagenes> result = await factory.SQL.QueryAsync<mdl_Carrusel_Imagenes>("Ventas.sp_Obtener_Fotografias_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
