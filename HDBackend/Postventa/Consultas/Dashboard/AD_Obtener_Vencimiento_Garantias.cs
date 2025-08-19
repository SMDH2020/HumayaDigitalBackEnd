using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Obtener_Vencimiento_Garantias
    {
        private string CadenaConexion;
        public AD_Obtener_Vencimiento_Garantias(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Dashboard_Vencimiento_Garantias>> ObtenerVencimientos(int ejercicio, int periodo_inicio, int periodo_fin, string whatsapp, string estado, string adr, string sucursal)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("whatsapp", whatsapp, System.Data.DbType.String);
                parametros.Add("estatus", estado, System.Data.DbType.String);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Dashboard_Vencimiento_Garantias> result = await factory.SQL.QueryAsync<mdl_Dashboard_Vencimiento_Garantias>("PixelCode.Posventa.sp_Obtener_Vencimiento_Garantias", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Precios_Garantias_porModelo>> ObtenerPrecios()
        {
            try
            {

                var parametros = new DynamicParameters();

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Precios_Garantias_porModelo> result = await factory.SQL.QueryAsync<mdl_Precios_Garantias_porModelo>("PixelCode.Posventa.sp_Obtener_Precio_Garantias_porModelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarPrecioGarantia(mdl_Agregar_Precio_Garantia mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    modelo = mdl.modelo,
                    precio_original = mdl.precio_original,
                    precio_ajustado = mdl.precio_ajustado,
                    inicio_vigencia = mdl.inicio_vigencia,
                    vigencia = mdl.vigencia,
                    usuario = mdl.usuario,
                };
                await factory.SQL.QueryAsync("PixelCode.Posventa.sp_Guardar_Precio_Garantia_porModelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Mensaje_Garantia>> ObtenerMensaje(string tipo)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("tipo", tipo, System.Data.DbType.String);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Mensaje_Garantia> result = await factory.SQL.QueryAsync<mdl_Mensaje_Garantia>("PixelCode.Posventa.sp_Obtener_Mensaje_Garantia", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarMensajeGarantia(mdl_Mensaje_Garantia mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    mensaje = mdl.mensaje,
                    tipo = mdl.tipo,
                    inicio_vigencia = mdl.inicio_vigencia,
                    vigencia = mdl.vigencia,
                    usuario = mdl.usuario,
                };
                await factory.SQL.QueryAsync("PixelCode.Posventa.sp_Guardar_Mensaje_Garantia", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Obtener_Modelos_Garantia>> ObtenerModelos()
        {
            try
            {
                var parametros = new DynamicParameters();
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Modelos_Garantia> result = await factory.SQL.QueryAsync<mdl_Obtener_Modelos_Garantia>("PixelCode.Posventa.sp_Obtener_Modelos_Garantia", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Obtener_Modelos_Garantia>> ExcluirModelo(string modelo,string tipo, int usuario)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("modelo", modelo, System.Data.DbType.String);
                parametros.Add("tipo", tipo, System.Data.DbType.String);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Modelos_Garantia> result = await factory.SQL.QueryAsync<mdl_Obtener_Modelos_Garantia>("PixelCode.Posventa.sp_Garantia_Modelos_Excluidos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Obtener_Modelos_Garantia>> EliminarReglaExclusion(string id)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id", id, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Modelos_Garantia> result = await factory.SQL.QueryAsync<mdl_Obtener_Modelos_Garantia>("PixelCode.Posventa.Garantia_Modelos_Excluidos_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
