using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Inventario;

namespace Usados.Modelos.Inventario
{
    public class AD_Listado_Precio_Obtener_Promocion
    {
        private string CadenaConexion;
        public AD_Listado_Precio_Obtener_Promocion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_promocion> BuscarID(int idinventario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinventario = idinventario,
                };
                mdl_promocion result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_promocion>("Usados.sp_Listado_Precio_Obtener_Promocion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
