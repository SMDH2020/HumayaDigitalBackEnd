using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Carga_Promedio_Duracion_Timeline
    {
        private string CadenaConexion;
        public AD_Carga_Promedio_Duracion_Timeline(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Promedio_Duracion_Timeline>> Promedio(int ejercicio, int periodo)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Promedio_Duracion_Timeline> result = await factory.SQL.QueryAsync<mdlCarga_Promedio_Duracion_Timeline>("Credito.sp_Obtener_Promedio_Tiempo_Timeline", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
