using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.Sala
{
    public class AD_Sala
    {
        private string CadenaConexion;

        public AD_Sala(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<int> Guardar(mdl_Sala sala )
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    id=sala.idsala,
                    nombreSala = sala.nombresala,
                    tipoSala=sala.tiposala,
                    user = sala.usuario,
                    estado =sala.estado
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                    "Seguimiento_Actividades..SP_Cat_Sala_Guardar",
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

        public async Task<List<mdl_Sala>> Listado()
        {   
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);


                var result = await factory.SQL.QueryAsync<mdl_Sala>(
                    "Seguimiento_Actividades..SP_Cat_Sala_Listado",
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

        public async Task<mdl_Sala> Obtener(int id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    id = id
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Sala>(
                    "Seguimiento_Actividades..SP_Cat_Sala_Obtener",
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