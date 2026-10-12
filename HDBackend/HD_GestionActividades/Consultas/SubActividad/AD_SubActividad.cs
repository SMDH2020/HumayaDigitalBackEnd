using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.SubActividad
{
    // Catálogo (plantilla) de subactividades por actividad -- CRUD simple,
    // mismo patrón que AD_GrupoActividades/AD_Actividad. No toca ninguna
    // tabla ni SP existente.
    public class AD_SubActividad
    {
        private string CadenaConexion;

        public AD_SubActividad(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Guardar(mdl_SubActividad subActividad)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    id = subActividad.idSubActividad,
                    idActividad = subActividad.idActividad,
                    nombreSubActividad = subActividad.nombreSubActividad,
                    orden = subActividad.orden,
                    estado = subActividad.estado,
                    user = subActividad.usuario
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                    "Seguimiento_Actividades..SP_Cat_SubActividad_Guardar",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<List<mdl_SubActividad>> Listado(int idActividad)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new { idActividad };

                var result = await factory.SQL.QueryAsync<mdl_SubActividad>(
                    "Seguimiento_Actividades..SP_Cat_SubActividad_Listado",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task EliminarPorID(int id, int user)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new { id, user };

                await factory.SQL.ExecuteAsync(
                    "Seguimiento_Actividades..SP_Cat_SubActividad_EliminarPorID",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }
    }
}
