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
    public class AD_Rotacion_Inventario_Reporte
    {
        private string CadenaConexion;
        public AD_Rotacion_Inventario_Reporte(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Rotacion_Inventario>> reporte(int ejercicio, int periodo, string adr, string sucursales)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursales = sucursales
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Rotacion_Inventario> result = await factory.SQL.QueryAsync<mdl_Rotacion_Inventario>("PixelCode.dbo.sp_Nivel_Inventario_Rotacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
