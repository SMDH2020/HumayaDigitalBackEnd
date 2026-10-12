using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas
{
    public class AD_Actividades
    {
        private readonly string CadenaConexion;
        public AD_Actividades(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Crear(mdl_ActividadCrear mdl, int creado_por)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    mdl.idsolicitud,
                    mdl.descripcion,
                    mdl.estimacion_horas,
                    mdl.idusuario_developer,
                    creado_por
                };
                var idactividad = await factory.SQL.QuerySingleAsync<int>(
                    "dbo.sp_Actividades_Crear", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return idactividad;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Actividad>> Listado(int idsolicitud)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Actividad>(
                    "dbo.sp_Actividades_Listado", new { idsolicitud }, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // Vista del developer: solo lo que le fue asignado a él.
        public async Task<IEnumerable<mdl_Actividad>> ListadoPorDeveloper(int idusuario_developer)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Actividad>(
                    "dbo.sp_Actividades_ListadoPorDeveloper", new { idusuario_developer }, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // El controller ya validó que idusuario sea el dueño de la actividad (o Admin).
        public async Task MarcarEstado(mdl_MarcarEstadoActividad mdl, int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { mdl.idactividad, mdl.estado, idusuario };
                await factory.SQL.ExecuteAsync("dbo.sp_Actividades_MarcarEstado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // Usado por el controller para validar dueño antes de MarcarEstado.
        public async Task<int?> ObtenerDeveloperDeActividad(int idactividad)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryFirstOrDefaultAsync<int?>(
                    "SELECT idusuario_developer FROM dbo.Actividades WHERE idactividad = @idactividad",
                    new { idactividad });
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
