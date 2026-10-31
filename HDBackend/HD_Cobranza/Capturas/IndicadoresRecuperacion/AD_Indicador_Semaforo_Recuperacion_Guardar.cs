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
    public class AD_Indicador_Semaforo_Recuperacion_Guardar
    {
        private string CadenaConexion;
        public AD_Indicador_Semaforo_Recuperacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Indicadores_Semaforo_Recuperacion> Guardar(mdl_Indicadores_Semaforo_Recuperacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idIndicador = mdl.idIndicador,
                    tipo_indicador = mdl.tipo_indicador,
                    tipo_cartera = mdl.tipo_cartera,
                    semaforo= mdl.semaforo,
                    ejercicio = mdl.ejercicio,
                    enero_minimo = mdl.enero_minimo,
                    enero_maximo = mdl.enero_maximo,
                    febrero_minimo = mdl.febrero_minimo,
                    febrero_maximo = mdl.febrero_maximo,
                    marzo_minimo = mdl.marzo_minimo,
                    marzo_maximo = mdl.marzo_maximo,
                    abril_minimo = mdl.abril_minimo,
                    abril_maximo = mdl.abril_maximo,
                    mayo_minimo = mdl.mayo_minimo,
                    mayo_maximo = mdl.mayo_maximo,
                    junio_minimo = mdl.junio_minimo,
                    junio_maximo = mdl.junio_maximo,
                    julio_minimo = mdl.julio_minimo,
                    julio_maximo = mdl.julio_maximo,
                    agosto_minimo = mdl.agosto_minimo,
                    agosto_maximo = mdl.agosto_maximo,
                    septiembre_minimo = mdl.septiembre_minimo,
                    septiembre_maximo = mdl.septiembre_maximo,
                    octubre_minimo = mdl.octubre_minimo,
                    octubre_maximo = mdl.octubre_maximo,
                    noviembre_minimo = mdl.noviembre_minimo,
                    noviembre_maximo = mdl.noviembre_maximo,
                    diciembre_minimo = mdl.diciembre_minimo,
                    diciembre_maximo = mdl.diciembre_maximo,
                    autoriza_gerencia_finanzas = mdl.autoriza_gerencia_finanzas,
                    autoriza_gerencia_cobranza = mdl.autoriza_gerencia_cobranza,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Indicadores_Semaforo_Recuperacion>("Cartera_Clientes.dbo.sp_Indicadores_Semaforo_Recuperacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdl_Indicadores_Semaforo_Recuperacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
