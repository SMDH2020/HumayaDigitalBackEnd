using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;
using System.Data;

namespace HD_Dashboard.Consultas.Vendedor
{
    public class Dash_Vendedor_Main
    {
        private string CadenaConexion;
        public Dash_Vendedor_Main(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlDashboard_Vendedor_Result> Dashboard()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                bool isDataInDB = true;
                mdlDashboard_Info? result_info;
                mdlDashboard_Vendedor_Byte? result_vendedor1;
                mdlDashboard_Vendedor_Byte? result_vendedor2;
                mdlDashboard_Vendedor_Byte? result_vendedor3;
                mdlDashboard_Vendedor_Result dashboard = new mdlDashboard_Vendedor_Result();

                var result = await factory.SQL.QueryMultipleAsync("dashboard.sp_Dashboard_Vendedor", commandType: CommandType.StoredProcedure);
                result_info = result.Read<mdlDashboard_Info>().FirstOrDefault();
                result_vendedor1 = result.Read<mdlDashboard_Vendedor_Byte>().FirstOrDefault();
                result_vendedor2 = result.Read<mdlDashboard_Vendedor_Byte>().FirstOrDefault();
                result_vendedor3 = result.Read<mdlDashboard_Vendedor_Byte>().FirstOrDefault();
                //if (result_vendedor1 == null || result_vendedor2 == null || result_vendedor3 == null)
                //{
                //VendedorDelMes.ObtenerVendedorDelMesExcel(factory);
                //}
                //else
                //{
                //    isDataInDB = true;
                //}



                //Vendedor 1
                mdlDashboard_Vendedor_Base64 vendedor1 = new mdlDashboard_Vendedor_Base64();
                vendedor1.idempleado = result_vendedor1.idempleado;
                vendedor1.nombrecompleto = result_vendedor1.nombrecompleto;
                vendedor1.sucursal = result_vendedor1.sucursal;
                vendedor1.foto = Convert.ToBase64String(result_vendedor1.foto);

                //Vendedor 2
                mdlDashboard_Vendedor_Base64 vendedor2 = new mdlDashboard_Vendedor_Base64();
                vendedor2.idempleado = result_vendedor2.idempleado;
                vendedor2.nombrecompleto = result_vendedor2.nombrecompleto;
                vendedor2.sucursal = result_vendedor2.sucursal;
                vendedor2.foto = Convert.ToBase64String(result_vendedor2.foto);

                //v
                mdlDashboard_Vendedor_Base64 vendedor3 = new mdlDashboard_Vendedor_Base64();
                vendedor3.idempleado = result_vendedor3.idempleado;
                vendedor3.nombrecompleto = result_vendedor3.nombrecompleto;
                vendedor3.sucursal = result_vendedor3.sucursal;
                vendedor3.foto = Convert.ToBase64String(result_vendedor3.foto);


                dashboard.informacion = result_info;
                dashboard.vendedor1 = vendedor1;
                dashboard.vendedor2 = vendedor2;
                dashboard.vendedor3 = vendedor3;
                factory.Close();
                return dashboard;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }




    }
}
