using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADCPInfoClientes
    {
        private string CadenaConexion;
        public ADCPInfoClientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlinfoView> Obtener(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                var result = await factory.SQL.QueryMultipleAsync("Cobranza.InfoConvenioCliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlinfoView mdlinfoView = new mdlinfoView();
                mdlinfoView.info = result.Read<mdlinfocliente>().FirstOrDefault();
                mdlinfoView.operacion = result.Read<mdlinfoSaldos>().FirstOrDefault();
                mdlinfoView.revolvente = result.Read<mdlinfoSaldos>().FirstOrDefault();

                factory.SQL.Close();
                return mdlinfoView;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
