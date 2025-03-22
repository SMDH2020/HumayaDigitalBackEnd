using Dapper;
using HD.AccesoDatos;
using ProductoAliado.Modelos.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoAliado.Consultas.Cotizacion
{
    public class AD_Cotizacion_PA_PDF
    {
        private string CadenaConexion;
        public AD_Cotizacion_PA_PDF(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> Cotizacion(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Listado_Precio_Actual_Movil", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
