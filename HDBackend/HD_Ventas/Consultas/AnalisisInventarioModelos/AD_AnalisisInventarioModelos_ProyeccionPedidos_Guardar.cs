using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.AnalisisInventarioModelos;
using HD_Ventas.Modelos.ObjetivosModelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.AnalisisInventarioModelos
{
    public class AD_AnalisisInventarioModelos_ProyeccionPedidos_Guardar
    {
        private string CadenaConexion;
        public AD_AnalisisInventarioModelos_ProyeccionPedidos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task Guardar(mdl_AnalisisInventarioModelos_ProyeccionPedidos_Guardar mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    //idsucursal = mdl.idsucursal,
                    // Cada elemento lleva su ejercicio dentro del JSON (idgeneral, ejercicio, periodo, cantidad)
                    json = JsonConvert.SerializeObject(mdl.proyeccion),
                    usuario = usuario
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_jd_pedidos_proyectados_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
