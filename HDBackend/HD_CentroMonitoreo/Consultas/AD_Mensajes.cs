using Dapper;
using HD.AccesoDatos;
using HD_CentroMonitoreo.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Consultas
{
    public class AD_Mensajes
    {
        private string CadenaConexion;

        public AD_Mensajes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<List<mdl_Mensajes>> MensajesCentroMonitoreo(int ejercicioinicio, int ejerciciofin, int periodoinicio, int periodofin, string adr, string sucursal, string estatus)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    ejercicioinicio = ejercicioinicio,
                    ejerciciofin = ejerciciofin,
                    periodoinicio = periodoinicio,
                    periodofin = periodofin,
                    adr = adr,
                    sucursal = sucursal,
                    estatus = estatus,

                };
                var result = await factory.SQL.QueryAsync<mdl_Mensajes>(
                    "HumayaDigital_Eventos.csc.Obtener_Listado_Mensajes",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_PDF_Mensajes> GetArchivoPDF(int orden, int organizacion)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    orden = orden,
                    organizacion = organizacion
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_PDF_Mensajes>(
                    "HumayaDigital_Eventos.csc.Get_PDF_Informe_Maquinaria_Mensajes",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }


    }
}
