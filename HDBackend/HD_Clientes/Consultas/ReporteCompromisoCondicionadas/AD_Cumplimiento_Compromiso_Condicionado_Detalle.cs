using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReporteCompromisoCondicionadas
{
    public class AD_Cumplimiento_Compromiso_Condicionado_Detalle
    {
        private string CadenaConexion;
        public AD_Cumplimiento_Compromiso_Condicionado_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Cumplimiento_Compromiso_Condicionado_Detalle>> Obtenerdetalle(int usuario, int ejercicio, int sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario,
                    ejercicio,
                    sucursal
                };
               IEnumerable <mdl_Cumplimiento_Compromiso_Condicionado_Detalle> result = await factory.SQL.QueryAsync<mdl_Cumplimiento_Compromiso_Condicionado_Detalle>("Credito.sp_Cumplimiento_Compromiso_Condicionado_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
