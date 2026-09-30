using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas
{
    public class AD_RolTI
    {
        private readonly string CadenaConexion;
        public AD_RolTI(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        // Regresa 'Usuario' si el idusuario no tiene fila explicita en RolesTI.
        public async Task<string> Obtener(int idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var rol = await factory.SQL.QueryFirstOrDefaultAsync<string>(
                    "dbo.sp_RolTI_Obtener", new { idusuario }, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return rol ?? "Usuario";
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task Asignar(mdl_RolTI_Asignar mdl, int asignadoPor)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { mdl.idusuario, mdl.rol, asignadoPor };
                await factory.SQL.ExecuteAsync("dbo.sp_RolTI_Asignar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_RolTI>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_RolTI>(
                    "dbo.sp_RolTI_Listado", commandType: System.Data.CommandType.StoredProcedure);
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
