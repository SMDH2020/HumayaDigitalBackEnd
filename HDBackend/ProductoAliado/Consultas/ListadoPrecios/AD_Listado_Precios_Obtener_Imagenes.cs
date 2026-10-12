using Dapper;
using HD.AccesoDatos;
using ProductoAliado.Modelos.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoAliado.Consultas.ListadoPrecios
{
    public class AD_Listado_Precios_Obtener_Imagenes
    {
        private string CadenaConexion;
        public AD_Listado_Precios_Obtener_Imagenes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Imagenes_Producto_Aliado>> ListadoFiltro(string idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Imagenes_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Imagenes_Producto_Aliado>("ProductoAliado.sp_Listado_Precios_Obtener_Imagenes", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
