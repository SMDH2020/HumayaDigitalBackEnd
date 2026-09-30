using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.RotacionInventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.RotacionInventario
{
    public class AD_RotacionCXC_Detalle
    {
        private string CadenaConexion;
        public AD_RotacionCXC_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_RotacionCXC_Detalle>> DetalleCXC(int ejercicio, int periodo, string adr, string sucursales, string departamentos, string? usuario, string tipoReporte)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursales = sucursales,
                    departamentos = departamentos,
                    usuario = usuario,
                    tipo_reporte = tipoReporte
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_RotacionCXC_Detalle> result = await factory.SQL.QueryAsync<mdl_RotacionCXC_Detalle>("EQUIP.fiscal.sp_get_rotacion_cxc", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
