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
    public class AD_Indicador_Recuperacion_Listado
    {
        private string CadenaConexion;
        public AD_Indicador_Recuperacion_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Indicadores_Recuperacion>> Listado(int ejercicio)
        {
            try
            {
                var parametros = new
                {
                    ejercicio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Indicadores_Recuperacion> result = await factory.SQL.QueryAsync<mdl_Indicadores_Recuperacion>("Cartera_Clientes.dbo.sp_Indicadores_Recuperacion_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                // Los 4 tipos fijos
                var tiposFaltantes = new List<string> { "O", "R", "E", "J" };

                // Asegurarse de que siempre haya un objeto por cada tipo de "tipo_cartera"
                var resultConTiposCompletos = tiposFaltantes.Select(tipo => 
                {
                    // Buscar el tipo en los datos obtenidos
                    var tipoExistente = result.FirstOrDefault(x => x.tipo_cartera == tipo);

                    if (tipoExistente != null)
                    {
                        // Si el tipo ya existe, devolverlo tal como está
                        return tipoExistente;
                    }
                    else
                    {
                        // Si no existe el tipo, crear un objeto nuevo con todos los meses en 0
                        return new mdl_Indicadores_Recuperacion
                        {
                            idIndicador = 0,
                            tipo_indicador = "RCO",
                            tipo_cartera = tipo,
                            ejercicio = ejercicio,
                            enero = 0,
                            febrero = 0,
                            marzo = 0,
                            abril = 0,
                            mayo = 0,
                            junio = 0,
                            julio = 0,
                            agosto = 0,
                            septiembre = 0,
                            octubre = 0,
                            noviembre = 0,
                            diciembre = 0,
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
