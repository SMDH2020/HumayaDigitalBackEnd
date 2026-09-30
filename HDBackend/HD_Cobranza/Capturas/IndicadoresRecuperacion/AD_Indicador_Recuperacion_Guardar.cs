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
    public class AD_Indicador_Recuperacion_Guardar
    {
        private string CadenaConexion;
        public AD_Indicador_Recuperacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Indicadores_Recuperacion> Guardar(mdl_Indicadores_Recuperacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idIndicador = mdl.idIndicador,
                    tipo_indicador = mdl.tipo_indicador,
                    tipo_cartera = mdl.tipo_cartera,
                    ejercicio = mdl.ejercicio,
                    enero = mdl.enero,
                    febrero = mdl.febrero,
                    marzo = mdl.marzo,
                    abril = mdl.abril,
                    mayo = mdl.mayo,
                    junio = mdl.junio,
                    julio = mdl.julio,
                    agosto = mdl.agosto,
                    septiembre = mdl.septiembre,
                    octubre = mdl.octubre,
                    noviembre = mdl.noviembre,
                    diciembre = mdl.diciembre,
                    autoriza_gerencia_finanzas = mdl.autoriza_gerencia_finanzas,
                    autoriza_gerencia_cobranza = mdl.autoriza_gerencia_cobranza,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Indicadores_Recuperacion>("Cartera_Clientes.dbo.sp_Indicadores_Recuperacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdl_Indicadores_Recuperacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
