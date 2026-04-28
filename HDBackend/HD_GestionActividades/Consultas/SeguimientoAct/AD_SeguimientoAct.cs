using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;
using System.Data;

namespace HD_GestionActividades.Consultas.SeguimientoAct
{
    public class AD_SeguimientoAct
    {
        private readonly string CadenaConexion;

        public AD_SeguimientoAct(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> GuardarAsync(mdl_SeguimientoAct seguimiento)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                id = seguimiento.idSolicitud,
                idSala = seguimiento.idSala,
                idActividad = seguimiento.idActividad,
                comentarios = seguimiento.comentarios,
                evidencia = seguimiento.evidencia,
                user = seguimiento.usuario,
                prioridad = seguimiento.prioridad
            };

            var idGenerado = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return idGenerado;
        }

        public async Task EditarAsync(mdl_SeguimientoAct seguimiento)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud = seguimiento.idSolicitud,
                idSala = seguimiento.idSala,
                idActividad = seguimiento.idActividad,
                comentarios = seguimiento.comentarios,
                evidencia = seguimiento.evidencia,
                estatus = seguimiento.estatus,
                prioridad = seguimiento.prioridad
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Editar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            var parametrosHistorial = new
            {
                idSolicitud = seguimiento.idSolicitud,
                estatus = seguimiento.estatus,
                user = seguimiento.usuario
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_CambiarEstatus",
                parametrosHistorial,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }


        public async Task<List<mdl_SeguimientoAct>> ListadoAsync(int idUsuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idUsuario
            };

            var result = await factory.SQL.QueryAsync<mdl_SeguimientoAct>(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Listado",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<mdl_SeguimientoAct> ObtenerAsync(int idSolicitud, int idUsuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud,
                idUsuario
            };

            var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_SeguimientoAct>(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Obtener",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<string> EliminarPorIDAsync(int idSolicitud, int user)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud,
                user
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_EliminarPorID",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return "Registro eliminado correctamente";
        }



        public async Task<List<mdl_SeguimientoAct>> HistorialAsync(int idSolicitud)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud
            };

            var result = await factory.SQL.QueryAsync<mdl_SeguimientoAct>(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Historial_Listado",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            return result.ToList();
        }


        public async Task AgregarComentarioAsync(int idSolicitud, string comentario, int user)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud,
                comentario,
                user
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_AgregarComentario",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<int> ConteoNoRevisadosAsync(int idUsuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            return await factory.SQL.QueryFirstOrDefaultAsync<int>(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_ConteoNoRevisados",
                new { idUsuario },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarcarRevisadoAsync(int idUsuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_MarcarRevisado",
                new { idUsuario },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task CalificarAsync(int idSolicitud, int calificacion, string comentario, int user)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud,
                calificacion,
                comentario,
                user
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Calificar",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ReactivarAsync(int idSolicitud, string comentario, int user)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSolicitud,
                comentario,
                user
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Cat_SeguimientoAct_Reactivar",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }   
    }

}