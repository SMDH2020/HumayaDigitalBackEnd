using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas
{
    public class AD_Bitacora
    {
        private readonly string CadenaConexion;
        public AD_Bitacora(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        // entidad: 'Solicitud' | 'Actividad'
        public async Task<IEnumerable<mdl_BitacoraEvento>> Historial(string entidad, int identidad)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new { entidad, identidad };
                var result = await factory.SQL.QueryAsync<mdl_BitacoraEvento>(
                    "dbo.sp_Bitacora_Historial", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
