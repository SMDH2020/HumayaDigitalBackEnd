using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PrecioLista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PrecioLineas
{
    public class AD_Precio_Lineas_Listado
    {
        private string CadenaConexion;
        public AD_Precio_Lineas_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Precio_Lineas>> Listado( int ejercicio, int sucursal)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    sucursal = sucursal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Precio_Lineas> result = await factory.SQL.QueryAsync<mdl_Precio_Lineas>("Ventas.sp_Precio_Lineas_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
