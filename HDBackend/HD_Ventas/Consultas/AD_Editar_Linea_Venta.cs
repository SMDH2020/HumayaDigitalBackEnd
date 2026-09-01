using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Editar_Linea_Venta
    {
        private string CadenaConexion;
        public AD_Editar_Linea_Venta(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListadoLineasVentas>> EditarLinea(mdlListadoLineasVentas mdl)
        {
            try
            {
                var parametros = new
                {
                    @idlinea = mdl.idlinea,
                    @descripcion = mdl.descripcion,
                    @estatus = mdl.estatus,
                    @usuario = mdl.usuario,
                    @departamento=mdl.departamento,
                    @imagen = mdl.imagen
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListadoLineasVentas> result = await factory.SQL.QueryAsync<mdlListadoLineasVentas>("Ventas.sp_Editar_Linea_Venta", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
