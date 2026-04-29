using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PrecioLineas
{
    public class AD_Precio_Linea_Guardar
    {
        private string CadenaConexion;
        public AD_Precio_Linea_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> Guardar_precio(int idlinea, int ejercicio, int periodo, int sucursal, double precio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idlinea = idlinea,
                    ejercicio = ejercicio,
                    periodo = periodo,
                    sucursal = sucursal,
                    precio = precio,
                    usuario = usuario
                };
                 await factory.SQL.ExecuteAsync("Ventas.sp_Precio_Lineas_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
