using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.ObjetivosModelos;

namespace HD_Ventas.Consultas.ObjetivosModelos
{
    public class AD_Objetivos_Modelos_Listado
    {
        private string CadenaConexion;
        public AD_Objetivos_Modelos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Objetivos_Modelos_Listado>> Listado(int ejercicio, string sucursal)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    sucursal = sucursal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Objetivos_Modelos_Listado> result = await factory.SQL.QueryAsync<mdl_Objetivos_Modelos_Listado>("Ventas.sp_Objetivos_Modelos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
