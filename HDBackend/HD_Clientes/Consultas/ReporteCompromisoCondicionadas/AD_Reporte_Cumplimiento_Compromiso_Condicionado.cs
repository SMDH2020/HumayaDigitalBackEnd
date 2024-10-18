using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReporteCompromisoCondicionadas
{
    public class AD_Reporte_Cumplimiento_Compromiso_Condicionado
    {
        private string CadenaConexion;
        public AD_Reporte_Cumplimiento_Compromiso_Condicionado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Cumplimiento_Compromiso_Condicionado_Vista> Obtener(int ejercicio, int sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio,
                    sucursal
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Cumplimiento_Compromiso_Condicionado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Cumplimiento_Compromiso_Condicionado_Vista mdl = new mdl_Cumplimiento_Compromiso_Condicionado_Vista();
                mdl.resumen = result.Read<mdl_Cumplimiento_Compromiso_Condicionado>().FirstOrDefault();
                mdl.detalle = result.Read<mdl_Cumplimiento_Compromiso_Condicionado_General>().ToList();
                if (mdl.resumen is null) mdl.resumen = new mdl_Cumplimiento_Compromiso_Condicionado();
                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
