using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.Actividad
{
    public class AD_Actividad
    {
        private string CadenaConexion;

        public AD_Actividad(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Guardar(mdl_Actividad actividad)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    id = actividad.idactividad,
                    idGrupoActividades = actividad.idgrupoactividades,
                    nombreActividad = actividad.nombreactividad,
                    sla = actividad.sla,
                    tiempoSolucion = actividad.tiemposolucion,
                    tiempo = actividad.tiempo,
                    user = actividad.usuario,
                    estado = actividad.estado
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                    "Seguimiento_Actividades..SP_Cat_Actividad_Guardar",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(
                    System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message }
                );
            }
        }

        public async Task<List<mdl_Actividad>> Listado(
            int? idGrupoActividades = null,
            string? nombreActividad = null,
            bool? estado = null)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    idGrupoActividades,
                    nombreActividad,
                    estado
                };

                var result = await factory.SQL.QueryAsync<mdl_Actividad>(
                    "Seguimiento_Actividades..SP_Cat_Actividad_Listado",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(
                    System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message }
                );
            }
        }

        public async Task<List<mdl_Actividad>> ActividadesPorSala(int idSala)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    idSala = idSala
                };

                var result = await factory.SQL.QueryAsync<mdl_Actividad>(
                    "Seguimiento_Actividades..SP_Sala_ActividadesPorSala",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();

                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(
                    System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message }
                );
            }
        }


        public async Task<mdl_Actividad> Obtener(int id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new { id = id };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Actividad>(
                    "Seguimiento_Actividades..SP_Cat_Actividad_Obtener",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(
                    System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message }
                );
            }
        }
    }
}