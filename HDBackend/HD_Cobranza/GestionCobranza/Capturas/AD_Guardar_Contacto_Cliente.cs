using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Guardar_Contacto_Cliente
    {
        private string CadenaConexion;
        public AD_Guardar_Contacto_Cliente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Guardar_Contacto_Cliente>> Guardar(mdl_Guardar_Contacto_Cliente mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idmedio = mdl.idmedio,
                    @idcliente = mdl.idcliente,
                    @mediocontacto = mdl.mediocontacto,
                    @medio = mdl.medio,
                    @comentarios = mdl.comentarios,
                    @usuario = mdl.usuario,
                    @responsable_pago = mdl.responsable_pago,
                };

                var result = await factory.SQL.QueryAsync<mdl_Guardar_Contacto_Cliente>("GestionCobranza.sp_Guardar_Contacto_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
