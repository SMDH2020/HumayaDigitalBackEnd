using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.RotacionCXC;
using HD_Finanzas.Modelos.RotacionInventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.RotacionCXC
{
    public class GuiaCXCListado
    {
        private string CadenaConexion;
        public GuiaCXCListado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_GuiaCXC_Listado>> Listado(int ejercicio, string tipo_ubi, int ubicacion)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    tipo_ubi = tipo_ubi,
                    ubicacion = ubicacion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_GuiaCXC_Listado> result = await factory.SQL.QueryAsync<mdl_GuiaCXC_Listado>("PixelCode.dbo.sp_Guia_CXC_Listado_P", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
