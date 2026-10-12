using Dapper;
using HD.AccesoDatos;

namespace HD.Clientes.Consultas.CRM.ObjetivosSemanales
{
    public class AD_ObjetivosSemanales_GenerarSemanas
    {
        private string CadenaConexion;
        public AD_ObjetivosSemanales_GenerarSemanas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Genera en la tabla todas las semanas del ejercicio.
        /// El SP debe insertar unicamente las semanas faltantes (idempotente).
        /// </summary>
        public async Task<bool> GenerarSemanas(int ejercicio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("CRM.sp_Objetivos_CRM_Generar_Semanas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
