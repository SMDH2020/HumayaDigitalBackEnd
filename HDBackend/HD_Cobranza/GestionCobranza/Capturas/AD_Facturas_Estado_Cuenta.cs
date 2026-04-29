using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
using System.Collections.Generic;
using System.Data;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Facturas_Estado_Cuenta
    {
        private string CadenaConexion;
        public AD_Facturas_Estado_Cuenta(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Facturas_Estado_Cuenta>> Get(string idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Facturas_Estado_Cuenta> impresion = await factory.SQL.QueryAsync<mdl_Facturas_Estado_Cuenta>("Cartera_Clientes.Cobranza.sp_Obtener_Facturas_Estado_Cuenta", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return impresion;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Facturas_Estado_Cuenta>> GetPorFecha(string idcliente,string Fecha, string usuario)
        {
            try
            {
                DateTime dt = DateTime.Parse(Fecha);
                var parametros = new DynamicParameters();
                parametros.Add("idcliente11", idcliente,DbType.String);
                parametros.Add("fecha", Fecha,DbType.String);
                parametros.Add("usuario", usuario, DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Facturas_Estado_Cuenta> impresion = await factory.SQL.QueryAsync<mdl_Facturas_Estado_Cuenta>("Cartera_Clientes.Cobranza.sp_Obtener_Facturas_Estado_Cuenta_PorFecha", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return impresion;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
