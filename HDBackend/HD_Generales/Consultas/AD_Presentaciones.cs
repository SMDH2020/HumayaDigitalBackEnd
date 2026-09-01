using Dapper;
using HD.AccesoDatos;
using HD.Generales.Modelos;

namespace HD.Generales.Consultas
{
    public class AD_Presentaciones
    {
        private string CadenaConexion;
        public AD_Presentaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mld_Presentaciones_Listado>> Guardar(mdl_Presentaciones_Guardar mdl)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = Guid.NewGuid(),
                    nombre = mdl.nombre,
                    descripcion = mdl.descripcion,
                    usuariocreacion = mdl.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mld_Presentaciones_Listado> result = await factory.SQL.QueryAsync<mld_Presentaciones_Listado>("HumayaDigital_Eventos.dbo.sp_Presentaciones_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mld_Presentaciones_Listado>> GuardarPresentacion(mdl_Presentaciones_Guardar_completo mdl)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = Guid.NewGuid(),
                    nombre = mdl.nombre,
                    descripcion = mdl.descripcion,
                    htmlContenido=mdl.htmlContenido,
                    usuariocreacion = mdl.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mld_Presentaciones_Listado> result = await factory.SQL.QueryAsync<mld_Presentaciones_Listado>("HumayaDigital_Eventos.dbo.sp_Presentaciones_Guardar_Presentacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mld_Presentaciones_Listado>> Actualizar(mdl_Presentaciones_Guardar mdl)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = mdl.presentacionId,
                    nombre = mdl.nombre,
                    descripcion = mdl.descripcion,
                    UsuarioModificacion = mdl.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mld_Presentaciones_Listado> result = await factory.SQL.QueryAsync<mld_Presentaciones_Listado>("HumayaDigital_Eventos.dbo.sp_Presentaciones_Modificar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<object> GuardarHtml(mdl_Presentaciones_Html mdl)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = mdl.presentacionId,
                    htmlcontenido = mdl.htmlContenido,
                    UsuarioModificacion = mdl.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("HumayaDigital_Eventos.dbo.sp_Presentaciones_ActualizarHtml", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new { mensaje = "Archivo cargado con exito" };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<object> Eliminar(Guid presentacionid)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = presentacionid
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("HumayaDigital_Eventos.dbo.sp_Presentaciones_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new { mensaje = "Presentacion Eliminada con exito" };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mld_Presentaciones_Listado> Buscar(Guid id)
        {
            try
            {
                var parametros = new
                {
                    PresentacionId = id
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mld_Presentaciones_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mld_Presentaciones_Listado>("HumayaDigital_Eventos.dbo.sp_Presentaciones_Buscar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mld_Presentaciones_Listado>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mld_Presentaciones_Listado> result = await factory.SQL.QueryAsync<mld_Presentaciones_Listado>("HumayaDigital_Eventos.dbo.sp_Presentaciones_Listar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
