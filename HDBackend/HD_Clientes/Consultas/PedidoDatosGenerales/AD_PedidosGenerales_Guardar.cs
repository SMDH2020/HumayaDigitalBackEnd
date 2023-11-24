using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoDatosGenerales
{
    public class AD_PedidosGenerales_Guardar
    {
        private string CadenaConexion;
        public AD_PedidosGenerales_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlPedido_Datos_Generales mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    solicitante = mdl.solicitante,
                    celular = mdl.celular,
                    correoelectronico = mdl.correoelectronico,
                    fechaentrega = mdl.fechaentrega,
                    domicilio = mdl.domicilio,
                    lugarentrega = mdl.lugarentrega,
                    condicionescredito = mdl.condicionescredito,
                    metodopago = mdl.metodopago,
                    formapago = mdl.formapago,
                    usocfdi = mdl.usocfdi,
                    tiporelacion = mdl.tiporelacion,
                    anticipos = mdl.anticipos,
                    foliosanticipos = mdl.foliosanticipos,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Pedidos_Datos_Solicitante_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
