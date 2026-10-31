using Dapper;
using HD.AccesoDatos;
using HD_GestionActividades.Modelos;

namespace HD_GestionActividades.Consultas.SalaResponsable
{
    public class AD_SalaResponsable
    {
        private string CadenaConexion;

        public AD_SalaResponsable(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> Guardar(mdl_SalaResponsable model)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                id = model.idRelSalaResponsable,
                idSala = model.idSala,
                IDEmpleado = model.IDEmpleado,
                user = model.usuario,
                estado = model.estado
            };

            var result = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                "Seguimiento_Actividades..SP_Rel_Sala_Responsable_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            factory.SQL.Close();
            return result;
        }

        public async Task<List<mdl_SalaResponsable>> Listado    (short idSala)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                idSala = idSala
            };

            var result = await factory.SQL.QueryAsync<mdl_SalaResponsable>(
                "Seguimiento_Actividades..SP_Rel_Sala_Responsable_Listado",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            factory.SQL.Close();
            return result.ToList();
        }

        public async Task<IEnumerable<mdl_SalaResponsable>> EliminarPorID(int idRelSalaResponsable, int user, int idSala)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    idRelSalaResponsable,
                    user = user,
                    idSala 
                };

                var result = await factory.SQL.QueryAsync<mdl_SalaResponsable>(
                    "Seguimiento_Actividades..SP_Rel_Sala_Responsable_EliminarPorID",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex) {
                Console.WriteLine( ex.ToString() );
            }
            return new List<mdl_SalaResponsable>();
        
        }

        public async Task<IEnumerable<dynamic>> EmpleadosDropDown()
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync(@"
        SELECT 
            IDEmpleado,
            Nombre,
            ApellidoPaterno,
            ApellidoMaterno
        FROM AppMH.dbo.Empleados
        WHERE Estatus = 1
        ORDER BY Nombre
    ");

            factory.SQL.Close();
            return result;
        }
    }
}