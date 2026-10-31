using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Categorias_Modelos
    {
        private string CadenaConexion;
        public AD_Categorias_Modelos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Categorias_Modelos>> Categorias()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Categorias_Modelos> result = await factory.SQL.QueryAsync<mdl_Categorias_Modelos>("ventas.sp_Obtener_Listado_Categorias", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Categorias_Modelos>> Categoriasid(int id)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_categoria", id, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Categorias_Modelos> result = await factory.SQL.QueryAsync<mdl_Categorias_Modelos>("ventas.sp_Obtener_Categoria_Modelos_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Agregar_Categoria_Modelo>> AgregarCategoria(mdl_Agregar_Categoria_Modelo mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("descripcion", mdl.descripcion, System.Data.DbType.String);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Agregar_Categoria_Modelo> result = await factory.SQL.QueryAsync<mdl_Agregar_Categoria_Modelo>("ventas.sp_Agregar_Categoria_Modelos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Categorias_Modelos>> Editar_Categoria(mdl_Categorias_Modelos mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_categoria", mdl.id_categoria, System.Data.DbType.Int16);
                parametros.Add("descripcion", mdl.descripcion, System.Data.DbType.String);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Categorias_Modelos> result = await factory.SQL.QueryAsync<mdl_Categorias_Modelos>("Ventas.sp_Editar_Categoria_Modelos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Categorias_Modelos>> Editar_Categoria_Estatus(mdl_Categorias_Modelos mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_categoria", mdl.id_categoria, System.Data.DbType.Int16);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Categorias_Modelos> result = await factory.SQL.QueryAsync<mdl_Categorias_Modelos>("Ventas.sp_Editar_Estatus_Categoria_Modelos", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
