using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Capturas.Dashboard
{
    public class AD_Dashboard_Items
    {
        private string CadenaConexion;
        public AD_Dashboard_Items(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_Objetivo_Total_View> ObtenerItems(int prmejercicio, int prmperiodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    @ejercicio=prmejercicio,
                //    @periodo=prmperiodo
                //};
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio11",prmejercicio,System.Data.DbType.Int16);
                parametros.Add("periodo11",prmperiodo,System.Data.DbType.Int16);
                //var result1 = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.Cargar_Dashboard_Cobranza",parametros, commandType: System.Data.CommandType.StoredProcedure);
                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.Dashboard_Total_Clientes", parametros, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 60);
                var view = new mdl_Dashboard_Objetivo_Total_View();
                view.objetivo_total = result.Read<mdl_Dashboard_Objetivo_Total>().ToList();
                view.objetivo_cartera = result.Read<mdl_Dashboard_Objetivo_Total>().ToList();
                view.objetivo_responsable = result.Read<mdl_Dashboard_Objetivo_Total>().ToList();
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
