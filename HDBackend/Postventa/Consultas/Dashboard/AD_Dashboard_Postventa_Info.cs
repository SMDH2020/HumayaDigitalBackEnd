using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Dashboard_Postventa_Info
    {
        private string CadenaConexion;
        public AD_Dashboard_Postventa_Info(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_View> ObtenerDashboard(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
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
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);


                var result = await factory.SQL.QueryMultipleAsync("PixelCode.Posventa.sp_dashboard", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Dashboard_View();
                view.servicio = result.Read<mdl_Dashboard_Servicio>().ToList();
                //view.columnas = result.Read<string>().FirstOrDefault();
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
