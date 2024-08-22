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
    public class AD_Credito_Condicionado_Aprobar_Facturacion
    {

        private string CadenaConexion;
        public AD_Credito_Condicionado_Aprobar_Facturacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_fecha_compromiso> Guardar(mdl_fecha_compromiso mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    folio = mdl.folio,
                    usuario = mdl.usuario,
                    comentarios = mdl.comentarios,
                };
                mdl_fecha_compromiso result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_fecha_compromiso>("Credito.sp_Solicitud_Credito_Validar_Documentacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
