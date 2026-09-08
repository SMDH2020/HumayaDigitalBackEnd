using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Agregar_Objeciones_Pago
    {
        private string CadenaConexion;
        public AD_Agregar_Objeciones_Pago(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Objecion_Pago>> Objecion(string objecion, int usuario)
        {
            try
            {
                var parametros = new
                {
                    @objecion = objecion,
                    @usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Objecion_Pago> result = await factory.SQL.QueryAsync<mdl_Objecion_Pago>("GestionCobranza.sp_Guardar_Objeciones_Pago", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
