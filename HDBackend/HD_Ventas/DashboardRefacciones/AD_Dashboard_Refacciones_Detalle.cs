using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.DashboardRefacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.DashboardRefacciones
{
    public class AD_Dashboard_Refacciones_Detalle
    {
        private string CadenaConexion;
        public AD_Dashboard_Refacciones_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_Refacciones_View> ObtenerDashboard(string? fechainicio, string? fechafin, string? vendedor, string? cliente, string comparativa, string? adr, string? sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};

                var parametros = new DynamicParameters();
                parametros.Add("fechainicio", fechainicio, System.Data.DbType.String);
                parametros.Add("fechafin", fechafin, System.Data.DbType.String);
                parametros.Add("vendedor", vendedor, System.Data.DbType.String);
                parametros.Add("cliente", cliente, System.Data.DbType.String);
                parametros.Add("comparativa", comparativa, System.Data.DbType.String);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.ventas.sp_Facturacion_Familia_Detalle_Dashboard", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Dashboard_Refacciones_View();
                view.detalle = result.Read<mdl_Dashboard_Refacciones>().ToList();
                view.familia_10 = result.Read<mdl_Dashboard_Refacciones>().ToList();
                view.clientes_10 = result.Read<mdl_Dashboard_Refacciones>().ToList();
                view.pendiente1_10 = result.Read<mdl_Dashboard_Refacciones>().ToList();
                view.pendiente2_10 = result.Read<mdl_Dashboard_Refacciones>().ToList();

                //view.tipo_cartera = result.Read<string>().FirstOrDefault();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
