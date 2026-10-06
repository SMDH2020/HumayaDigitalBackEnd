using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using Postventa.Modelos.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.Indicadores
{
    public class AD_Indicadores_Posventa
    {
        private string CadenaConexion;
        public AD_Indicadores_Posventa(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Indicadores_Postventa_View> ObtenerIndicadores(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string seccion, string sucursales, string adr)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new DynamicParameters();
                parametros.Add("ejercicio_inicio", ejercicio_inicio, System.Data.DbType.Int16);
                parametros.Add("ejercicio_fin", ejercicio_fin, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("Seccion", seccion, System.Data.DbType.String);
                parametros.Add("Sucursales", sucursales, System.Data.DbType.String);
                parametros.Add("Adr", adr, System.Data.DbType.String);

                var result = await factory.SQL.QueryMultipleAsync("Postventa.sp_indicadores_posventa", parametros, commandTimeout: 120, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Indicadores_Postventa_View();
                view.Header = result.Read<mdl_Header_Indicadores>().FirstOrDefault();
                view.TopMasFacturan = result.Read<mdl_Tops_Responsables_Indicadores>().ToList();
                view.TopMenosFacturan = result.Read<mdl_Tops_Responsables_Indicadores>().ToList();
                view.Detalle = result.Read<mdl_Indicadores_Detalle>().ToList();

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
