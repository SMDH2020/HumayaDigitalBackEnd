using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Listado_Modelos
    {
        private string CadenaConexion;
        public AD_Listado_Modelos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> Listado()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Obtener_Modelos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> ListadoCompleto()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Listado_Modelos_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> ListadoCompletoenEsquema(int idpromocion)
        {
            try
            {
                var parametros = new
                {
                    idpromocion = idpromocion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Listado_Modelos_Cotizacion_enEsquema", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Lineas_DropDownList>> ListadoLineasDropdownlist(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Lineas_DropDownList> result = await factory.SQL.QueryAsync<mdl_Listado_Lineas_DropDownList>("Ventas.sp_Obtener_Lineas_Dropdownlist_Permisos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Categorias_DropDownList>> ListadoCategoriasDropdownlist()
        {
            try
            {
                var parametros = new
                {
                    
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Categorias_DropDownList> result = await factory.SQL.QueryAsync<mdl_Categorias_DropDownList>("Ventas.sp_Get_Categorias_DropDownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> ObtenerModeloID(int idmodelo)
        {
            try
            {
                var parametros = new
                {
                    @idmodelo = idmodelo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Obtener_Modelos_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
