using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos.Clientes;

namespace HD_Dashboard.Consultas
{
    public class Dash_Clientes_Main
    {
        private string CadenaConexion;
        public Dash_Clientes_Main(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlDashboard_Clientes> Dashboard(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Dashboard.sp_Clientes_general", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlDashboard_Clientes ctl = new mdlDashboard_Clientes();

                ctl.info = result.Read<mdlDashClientes_info>().FirstOrDefault();
                ctl.documentos = result.Read<mdlDashClientes_Documentos>().ToList();
                ctl.linea = result.Read<mdlDashClientes_Linea>().ToList();
                ctl.equipofacturado = result.Read<mdlEquipoFacturado>().ToList();
                ctl.cultivos = result.Read<mdlDashCultivos>().ToList();

                ctl.totalcredito = new mdlDashClientes_LineaTotales()
                {
                    porvencer= ctl.linea.Sum(item => item.porvencer),
                    vencido= ctl.linea.Sum(item => item.vencido),
                    total= ctl.linea.Sum(item => item.importe)
            };


                ctl.info.saldo= ctl.info.limitecredito - ctl.linea.Where(item => !item.linea.Equals("MAQ. NUEVA"))
                            .Sum(item => item.importe);


                factory.SQL.Close();
                return ctl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
