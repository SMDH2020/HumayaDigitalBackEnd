using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.NotasDescuento;

namespace HD_Cobranza.Capturas.NotasDescuento
{
    public class AD_Notas_Descuento
    {
        private string CadenaConexion;
        public AD_Notas_Descuento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Notas_Descuento>> ObtenerNotas(string adr, string sucursal)
        {
            try
            {
                var parametros = new DynamicParameters();
                //parametros.Add("ejercicio_inicio", ejercicio_inicio, System.Data.DbType.Int16);
                //parametros.Add("ejercicio_fin", ejercicio_fin, System.Data.DbType.Int16);
                //parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                //parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Notas_Descuento> result = await factory.SQL.QueryAsync<mdl_Notas_Descuento>("EQUIP.Cobranza.sp_Obtener_Notas_Descuento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
