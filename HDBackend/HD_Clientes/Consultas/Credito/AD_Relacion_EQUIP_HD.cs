using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Credito;
using HD.Clientes.Modelos.Especiales;

namespace HD.Clientes.Consultas.Credito
{
    public class AD_Relacion_EQUIP_HD
    {
        private string CadenaConexion;
        public AD_Relacion_EQUIP_HD(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Relacion_EQUIP_HD>> relacion(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @folio = folio
                };
                IEnumerable<mdl_Relacion_EQUIP_HD> result = await factory.SQL.QueryAsync<mdl_Relacion_EQUIP_HD>("Credito.sp_Relacion_EQUIP_HD", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> actualizar(mdl_Actualizar_Relacion_EQUIP_HD mdl)
        {
            try
            {
                var parametros = new
                {
                    @folio = mdl.folio,
                    @registro = mdl.registro,
                    @orden = mdl.orden,
                    @docto = mdl.docto
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documento_factura_Actualiza_financiamiento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
