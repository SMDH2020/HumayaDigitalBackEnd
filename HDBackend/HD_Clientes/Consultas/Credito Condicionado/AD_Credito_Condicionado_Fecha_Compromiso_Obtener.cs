using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Fecha_Compromiso_Obtener
    {
        private string CadenaConexion;
        public AD_Credito_Condicionado_Fecha_Compromiso_Obtener(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_fecha_compromiso> Get(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_fecha_compromiso result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_fecha_compromiso>("Credito.sp_Solicitud_Credito_Documentacion_Fecha_Compromiso_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
