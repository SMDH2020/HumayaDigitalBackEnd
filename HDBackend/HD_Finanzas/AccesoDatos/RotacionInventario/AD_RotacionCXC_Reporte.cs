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
    public class AD_RotacionCXC_Reporte
    {
        private string CadenaConexion;
        public AD_RotacionCXC_Reporte(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_RotacionCXC_View> reporte(int ejercicio, int periodo, string adr, string sucursales, string? usuario)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursales = sucursales,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //mdl_RotacionCXC_View result = await factory.SQL.QueryAsync<mdl_RotacionCXC_View>("PixelCode.dbo.sp_Rotacion_CXC", parametros, commandType: System.Data.CommandType.StoredProcedure);
                //factory.SQL.Close();
                //return result;

                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.sp_Rotacion_CXC", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_RotacionCXC_View reporte = new mdl_RotacionCXC_View();
                reporte.editor_guia = result.Read<bool>().FirstOrDefault();
                reporte.rotacion = result.Read<mdl_RotacionCXC>().ToList();
                factory.SQL.Close();
                return reporte;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
