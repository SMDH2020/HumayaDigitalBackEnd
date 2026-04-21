using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.RotacionCXC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.RotacionCXC
{
    public class AD_GuiaCXC_Guardar
    {
        private string CadenaConexion;
        public AD_GuiaCXC_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_GuiaCXC_Guardar mdl)
        {
            try
            {
                var parametros = new
                {
                   detalle = mdl.detalle,
                   usuario = mdl.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("PixelCode.dbo.sp_Guia_CXC_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
