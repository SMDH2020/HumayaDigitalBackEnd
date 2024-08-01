using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class AD_Clientes_Responsable_Cobranza
    {
        private string CadenaConexion;
        public AD_Clientes_Responsable_Cobranza(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Responsable_Cobranza>>

            Guardar(mdlClientes_Responsable_Cobranza mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @idusuario = mdl.idusuario,
                    @usuario = mdl.usuario,
                };

                var result = await
                factory.SQL.QueryAsync<mdlClientes_Responsable_Cobranza>("Credito.sp_Cliente_Responsable_Cobranza_Guardar",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }
    }
}
