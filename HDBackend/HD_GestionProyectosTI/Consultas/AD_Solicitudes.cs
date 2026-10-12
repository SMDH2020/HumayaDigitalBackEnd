using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas
{
    public class AD_Solicitudes
    {
        private readonly string CadenaConexion;
        public AD_Solicitudes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Crear(mdl_SolicitudCrear mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var idsolicitud = await factory.SQL.QuerySingleAsync<int>(
                    "dbo.sp_Solicitudes_Crear", mdl, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return idsolicitud;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // rol: 'Usuario' | 'Developer' | 'Admin' — controla qué ve cada quien.
        public async Task<IEnumerable<mdl_Solicitud>> Listado(int idusuario, string rol, string? estado, string? tipo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { idusuario, rol, estado, tipo };
                var result = await factory.SQL.QueryAsync<mdl_Solicitud>(
                    "dbo.sp_Solicitudes_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Solicitud?> Obtener(int idsolicitud)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync(
                    "dbo.sp_Solicitudes_Obtener", new { idsolicitud }, commandType: System.Data.CommandType.StoredProcedure);

                var solicitud = result.Read<mdl_Solicitud>().FirstOrDefault();
                var actividades = result.Read<mdl_Actividad>().ToList();
                factory.SQL.Close();

                if (solicitud != null)
                    solicitud.actividades = actividades;

                return solicitud;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task CambiarEstado(mdl_CambioEstado mdl, int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { mdl.idsolicitud, mdl.estado_nuevo, idusuario, mdl.motivo };
                await factory.SQL.ExecuteAsync("dbo.sp_Solicitudes_CambiarEstado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task Priorizar(mdl_Priorizar mdl, int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    mdl.idsolicitud,
                    mdl.prioridad,
                    mdl.fecha_estimada,
                    mdl.priorizado_con,
                    mdl.comentario_priorizacion,
                    idusuario,
                    mdl.motivo
                };
                await factory.SQL.ExecuteAsync("dbo.sp_Solicitudes_Priorizar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task CambiarFechaComprometida(mdl_CambioFecha mdl, int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { mdl.idsolicitud, mdl.fecha_nueva, idusuario, mdl.motivo };
                await factory.SQL.ExecuteAsync("dbo.sp_Solicitudes_CambiarFechaComprometida", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
