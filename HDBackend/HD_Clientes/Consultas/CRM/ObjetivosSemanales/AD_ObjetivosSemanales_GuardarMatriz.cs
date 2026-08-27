using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.ObjetivosSemanales;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD.Clientes.Consultas.CRM.ObjetivosSemanales
{
    public class AD_ObjetivosSemanales_GuardarMatriz
    {
        private string CadenaConexion;
        public AD_ObjetivosSemanales_GuardarMatriz(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Guarda la matriz de objetivos por linea. Solo se serializa el arreglo matriz,
        /// sin envoltorio. El SP maneja su propia transaccion y hace upsert por idlinea,
        /// por lo que no se abre transaccion adicional aqui.
        /// Los errores 50001 a 50008 son validaciones del SP con mensaje para el usuario
        /// final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task<bool> GuardarMatriz(mdl_ObjetivosSemanales_GuardarMatriz mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    actualiza_vendedor = mdl.actualiza_vendedor,
                    json = JsonConvert.SerializeObject(mdl.matriz),
                    usuario = mdl.usuario
                };
                await factory.SQL.ExecuteAsync("CRM.sp_ObjetivosSemanales_GuardarMatriz", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (SqlException ex) when (ex.Number >= 50001 && ex.Number <= 50008)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
