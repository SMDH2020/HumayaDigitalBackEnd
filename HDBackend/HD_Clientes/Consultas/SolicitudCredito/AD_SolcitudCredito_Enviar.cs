using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolcitudCredito_Enviar
    {
        private string CadenaConexion;
        public AD_SolcitudCredito_Enviar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitudCredito_Enviar> Detalle(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitudCredito_Enviar>("Credito.sp_Solicitud_Credito_Enviar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
