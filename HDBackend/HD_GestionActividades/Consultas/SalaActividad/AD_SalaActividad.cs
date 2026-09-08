using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.SalaActividad
{
    public class AD_SalaActividad
    {
        private string CadenaConexion;

        public AD_SalaActividad(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }


        public async Task Guardar(short idSala, short idActividad, short idUsuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSala,
                idActividad,
                idUsuario
            };

            await factory.SQL.ExecuteAsync(
                "Seguimiento_Actividades..SP_Rel_Sala_Actividad_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            factory.SQL.Close();
        }

        public async Task<List<dynamic>> Listado(short idSala)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSala
            };

            var result = await factory.SQL.QueryAsync(
                "Seguimiento_Actividades..SP_Rel_Sala_Actividad_Listado",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            factory.SQL.Close();
            return result.ToList();
        }


        public async Task<IEnumerable<dynamic>> EliminarPorID(int idRelSalaActividad, int user, int idSala)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    idRelSalaActividad,
                    user,
                    idSala
                };

                var result = await factory.SQL.QueryAsync(
                    "Seguimiento_Actividades..SP_Rel_Sala_Actividad_EliminarPorID",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new List<dynamic>();
        }
    }
}