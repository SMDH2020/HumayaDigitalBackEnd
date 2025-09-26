using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Obtener_Servicios_Pendientes
    {
        private string CadenaConexion;
        public AD_Obtener_Servicios_Pendientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Servicios_Pendientes>> ObtenerServicios(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal, int hrsuso, string msj_estatus, string motivo, string facturado)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);
                parametros.Add("hrsuso", hrsuso, System.Data.DbType.Int16);
                parametros.Add("msj_estatus", msj_estatus, System.Data.DbType.String);
                parametros.Add("motivo", motivo, System.Data.DbType.String);
                parametros.Add("facturado", facturado, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Servicios_Pendientes> result = await factory.SQL.QueryAsync<mdl_Servicios_Pendientes>("Postventa.sp_Obtener_Listado_Servicios_Pendientes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarContactoServiciosPendientes(mdl_Agregar_Contacto_Servicios_Pendientes mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_registro = mdl.id_registro,
                    contacto = mdl.contacto
                };
                await factory.SQL.QueryAsync("Postventa.sp_Añadir_Contacto_Servicios_Pendientes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }



        public async Task<IEnumerable<mdl_Paquetes_Mantenimiento>> ObtenerPaquetesMantenimiento()
        {
            try
            {
                var parametros = new DynamicParameters();
                
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Paquetes_Mantenimiento> result = await factory.SQL.QueryAsync<mdl_Paquetes_Mantenimiento>("Postventa.sp_Obtener_Paquetes_Disponibles", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Paquetes_Mantenimiento>> ObtenerPaquetesMantenimientoid(int id_paquete)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_paquete", id_paquete, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Paquetes_Mantenimiento> result = await factory.SQL.QueryAsync<mdl_Paquetes_Mantenimiento>("Postventa.sp_Obtener_Paquete_Editar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarPaqueteMantenimiento(mdl_Paquetes_Mantenimiento mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_paquete = mdl.id_paquete,
                    nombre = mdl.paquete,
                    periocidad = mdl.periocidad,
                    fecha = mdl.fecha,
                    contenido = mdl.contenido
                };
                await factory.SQL.QueryAsync("Postventa.sp_Guardar_Paquete_Mantenimiento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Precios_Mantenimiento_porModelo>> ObtenerPreciosMantenimiento(int id_paquete)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_paquete", id_paquete, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Precios_Mantenimiento_porModelo> result = await factory.SQL.QueryAsync<mdl_Precios_Mantenimiento_porModelo>("Postventa.sp_Obtener_Listado_Precios_porPaquete", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Precios_Mantenimiento_porModelo>> ObtenerPrecioMantenimientoModelo(int id_precio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_precio", id_precio, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Precios_Mantenimiento_porModelo> result = await factory.SQL.QueryAsync<mdl_Precios_Mantenimiento_porModelo>("Postventa.sp_Get_Info_Mantenimiento_porModelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarPreciosMantenimiento(mdl_Precios_Mantenimiento_porModelo mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("modelo", mdl.modelo, System.Data.DbType.String);
                parametros.Add("id_paquete", mdl.id_paquete, System.Data.DbType.Int16);
                parametros.Add("precio", mdl.precio, System.Data.DbType.Decimal);

                await factory.SQL.QueryAsync("Postventa.sp_Guardar_Precios_Mantenimiento_porModelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
