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
    public class AD_Listado_Precio_Promocion_Borrar
    {
        private string CadenaConexion;
        public AD_Listado_Precio_Promocion_Borrar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_promocion> Borrar(int idpromocion)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                   idpromocion = idpromocion,
                };
                mdl_promocion result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_promocion>("Usados.sp_Listado_Precio_Promocion_Borrar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                if (result == null)
                {
                    result = new mdl_promocion
                    {
                        idpromocion = -99,
                        idinventario = 0,
                        descripcion = "", 
                        vigencia = "",    
                        usuario = ""      
                    };
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
