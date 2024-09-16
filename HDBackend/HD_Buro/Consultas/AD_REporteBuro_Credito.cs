
using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Buro.Consultas
{
    public class AD_REporteBuro_Credito
    {
        private string CadenaConexion;
        public AD_REporteBuro_Credito(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDatosReporteBuro>> reporte(int ejercicio, int periodo)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = ejercicio,
                    Periodo = periodo,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDatosReporteBuro> result = await factory.SQL.QueryAsync<mdlDatosReporteBuro>("Cartera_Clientes.dbo.sp_Buro_Credito_Informe_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
