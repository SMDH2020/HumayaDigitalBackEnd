using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.IndicadoresRecuperacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.IndicadoresRecuperacion
{
    public class AD_Indicadores_Semaforo_Recuperacion_Listado
    {
        private string CadenaConexion;
        public AD_Indicadores_Semaforo_Recuperacion_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Indicadores_Semaforo_Recuperacion>> Listado(int ejercicio, string? tipo_cartera)
        {
            try
            {
                var parametros = new
                {
                    ejercicio,
                    tipo_cartera
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Indicadores_Semaforo_Recuperacion> result = await factory.SQL.QueryAsync<mdl_Indicadores_Semaforo_Recuperacion>("Cartera_Clientes.dbo.sp_Indicadores_Semaforo_Recuperacion_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                // Los 4 tipos fijos
                var tiposFaltantes = new List<string> { "V ", "A ", "R ", "RR" };

                // Asegurarse de que siempre haya un objeto por cada tipo de "tipo_cartera"
                var resultConTiposCompletos = tiposFaltantes.Select(tipo =>
                {
                    // Buscar el tipo en los datos obtenidos
                    var tipoExistente = result.FirstOrDefault(x => x.semaforo == tipo);

                    if (tipoExistente != null)
                    {
                        // Si el tipo ya existe, devolverlo tal como está
                        return tipoExistente;
                    }
                    else
                    {
                        // Si no existe el tipo, crear un objeto nuevo con todos los meses en 0
                        return new mdl_Indicadores_Semaforo_Recuperacion
                        {
                            idIndicador = 0,
                            tipo_indicador = "RCO",
                            tipo_cartera = tipo_cartera,
                            semaforo = tipo,
                            ejercicio = ejercicio,
                            enero_minimo = 0,
                            enero_maximo = 0,
                            febrero_minimo = 0,
                            febrero_maximo = 0,
                            marzo_minimo = 0,
                            marzo_maximo = 0,
                            abril_minimo = 0,
                            abril_maximo = 0,
                            mayo_minimo = 0,
                            mayo_maximo = 0,
                            junio_minimo = 0,
                            junio_maximo = 0,
                            julio_minimo = 0,
                            julio_maximo = 0,
                            agosto_minimo = 0,
                            agosto_maximo = 0,
                            septiembre_minimo = 0,
                            septiembre_maximo = 0,
                            octubre_minimo = 0,
                            octubre_maximo = 0,
                            noviembre_minimo = 0,
                            noviembre_maximo = 0,
                            diciembre_minimo = 0,
                            diciembre_maximo = 0,
                            autoriza_gerencia_finanzas = false,
                            autoriza_gerencia_cobranza = false,
                            usuario = "0"
                        };
                    }
                }).ToList();

                return resultConTiposCompletos;
                //return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
