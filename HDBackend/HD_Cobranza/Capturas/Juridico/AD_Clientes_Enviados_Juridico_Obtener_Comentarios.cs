using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.Juridico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Consultas.Juridico
{
    public class AD_Clientes_Enviados_Juridico_Obtener_Comentarios
    {
        private string CadenaConexion;
        public AD_Clientes_Enviados_Juridico_Obtener_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Clientes_Juridico_Guardar> obtener(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                mdl_Clientes_Juridico_Guardar result = await factory.SQL.QueryFirstAsync<mdl_Clientes_Juridico_Guardar>("Credito.sp_Clientes_Enviados_Juridico_Obtener_Comentarios", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
