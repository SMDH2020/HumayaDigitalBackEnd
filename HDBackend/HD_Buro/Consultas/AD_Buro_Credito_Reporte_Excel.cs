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
    public class AD_Buro_Credito_Reporte_Excel
    {
        private string CadenaConexion;
        public AD_Buro_Credito_Reporte_Excel(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Buro_Credito>> reporte(int ejercicio, int periodo)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = ejercicio,
                    Periodo = periodo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Buro_Credito> result = await factory.SQL.QueryAsync<mdl_Buro_Credito>("BuroCredito.dbo.sp_Buro_Credito_Reporte_Excel", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
