using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;
using HD_Ventas.Modelos.SolicitudesCerradas;

namespace HD_Ventas.Consultas
{
    public class AD_Promociones_Disponibles
    {
        private string CadenaConexion;
        public AD_Promociones_Disponibles(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Promociones_Disponibles>> ObtenerPromocionesDisponibles(string estado)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("estado", estado, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Promociones_Disponibles> result = await factory.SQL.QueryAsync<mdl_Promociones_Disponibles>("Ventas.sp_Obtener_Promociones_Disponibles", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Promociones_Disponibles>> ObtenerPromocionID(int idpromocion)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Promociones_Disponibles> result = await factory.SQL.QueryAsync<mdl_Promociones_Disponibles>("Ventas.sp_Obtener_Promocion_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Promociones_Disponibles>> AgregarPromocion(string descripcion, string inicio_vigencia, string vigencia, int usuario)
        {
            try
            {
                //var parametros = new
                //{
                //    @descripcion = descripcion,
                //    @inicio_vigencia = inicio_vigencia,
                //    @vigencia = vigencia,
                //    @usuario = usuario
                //};

                var parametros = new DynamicParameters();
                parametros.Add("descripcion_promocion", descripcion, System.Data.DbType.String);
                parametros.Add("inicio_vigencia", inicio_vigencia, System.Data.DbType.String);
                parametros.Add("vigencia", vigencia, System.Data.DbType.String);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Promociones_Disponibles> result = await factory.SQL.QueryAsync<mdl_Promociones_Disponibles>("Ventas.sp_Guardar_Promocion_Disponible", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Promociones_Disponibles>> EditarPromocion(int idpromocion, string descripcion, string inicio_vigencia, string vigencia, int usuario)
        {
            try
            {
                //var parametros = new
                //{
                //    @descripcion = descripcion,
                //    @inicio_vigencia = inicio_vigencia,
                //    @vigencia = vigencia,
                //    @usuario = usuario
                //};

                var parametros = new DynamicParameters();
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                parametros.Add("descripcion_promocion", descripcion, System.Data.DbType.String);
                parametros.Add("inicio_vigencia", inicio_vigencia, System.Data.DbType.String);
                parametros.Add("vigencia", vigencia, System.Data.DbType.String);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Promociones_Disponibles> result = await factory.SQL.QueryAsync<mdl_Promociones_Disponibles>("Ventas.sp_Editar_Promocion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Modelos_Esquema>> ObtenerModelosEsquema(int idpromocion)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Modelos_Esquema> result = await factory.SQL.QueryAsync<mdl_Modelos_Esquema>("Ventas.sp_Obtener_Modelos_Esquema", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarModelosEsquema(int idmodelo, int idpromocion, float costo_refacciones, float costo_servicios, float precio_promocion, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("idmodelo", idmodelo, System.Data.DbType.Int16);
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                parametros.Add("costo_refacciones", costo_refacciones, System.Data.DbType.Decimal);
                parametros.Add("costo_servicios", costo_servicios, System.Data.DbType.Decimal);
                parametros.Add("precio_promocion", precio_promocion, System.Data.DbType.Decimal);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);

                await factory.SQL.QueryAsync("Ventas.sp_Guardar_Modelos_Esquema", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Modelos_en_Esquema>> EliminarModelodeEsquema(int idmodelo, int idpromocion, int usuario)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("idmodelo", idmodelo, System.Data.DbType.Int16);
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Modelos_en_Esquema> result = await factory.SQL.QueryAsync<mdl_Modelos_en_Esquema>("Ventas.sp_Eliminar_Modelo_dePromocion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Modelos_en_Esquema>> RestaurarModelodeEsquema(int idmodelo, int idpromocion)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("idmodelo", idmodelo, System.Data.DbType.Int16);
                parametros.Add("idpromocion", idpromocion, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Modelos_en_Esquema> result = await factory.SQL.QueryAsync<mdl_Modelos_en_Esquema>("Ventas.sp_Restaurar_Modelo_dePromocion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Esquema_Linea_View> ObtenerModelosEnEsquema(int idlinea, int esquema)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("idlinea", idlinea, System.Data.DbType.Int16);
                parametros.Add("esquema", esquema, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Ventas.sp_Obtener_Modelos_En_Esquema_Linea", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Esquema_Linea_View mdl = new mdl_Esquema_Linea_View();
                mdl.modelos = result.Read<mdl_Modelos_en_Esquema>().ToList();
                mdl.esquemas = result.Read<mdl_Esquemas_por_Modelo>().ToList();

                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Modelos_en_Esquema>> ObtenerModelosEnEsquemaPDF(int idlinea, int esquema)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("idlinea", idlinea, System.Data.DbType.Int16);
                parametros.Add("esquema", esquema, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Modelos_en_Esquema> result = await factory.SQL.QueryAsync<mdl_Modelos_en_Esquema>("Ventas.sp_Obtener_Modelos_En_Esquema_Linea_PDF", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Esquemas_por_Modelo>> ObtenerEsquemasporModelo(int idmodelo)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("idmodelo", idmodelo, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Esquemas_por_Modelo> result = await factory.SQL.QueryAsync<mdl_Esquemas_por_Modelo>("Ventas.sp_Obtener_Esquemas_porModelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Esquemas_DDL>> GetEsquemasDDL()
        {
            try
            {

                var parametros = new DynamicParameters();
                //parametros.Add("idmodelo", idmodelo, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Esquemas_DDL> result = await factory.SQL.QueryAsync<mdl_Esquemas_DDL>("Ventas.sp_Get_Esquemas_Disponibles_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
