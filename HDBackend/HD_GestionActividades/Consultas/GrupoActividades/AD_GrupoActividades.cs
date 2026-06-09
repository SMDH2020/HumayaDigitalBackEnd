using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.GrupoActividades
{
    public class AD_GrupoActividades
    {
        private string CadenaConexion;

        public AD_GrupoActividades(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Guardar(mdl_GrupoActividades grupo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    id = grupo.idgrupoactividades,
                    nombreGrupoActividades = grupo.nombregrupoactividades,
                    user = grupo.usuario,
                    estado = grupo.estado
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                    "Seguimiento_Actividades..SP_Cat_GrupoActividades_Guardar",
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

        public async Task<List<mdl_GrupoActividades>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryAsync<mdl_GrupoActividades>(
                    "Seguimiento_Actividades..SP_Cat_GrupoActividades_Listado",
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

        public async Task<mdl_GrupoActividades> Obtener(int id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new { id = id };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_GrupoActividades>(
                    "Seguimiento_Actividades..SP_Cat_GrupoActividades_Obtener",
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
    }
}