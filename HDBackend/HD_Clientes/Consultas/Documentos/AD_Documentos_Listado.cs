using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Documentos
{
    public class AD_Documentos_Listado
    {
        private string CadenaConexion;
        public AD_Documentos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDocumentos>> Listado(int jdf)
        {
            try
            {
                var parametros = new
                {
                    jdf
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDocumentos> result = await factory.SQL.QueryAsync<mdlDocumentos>("Credito.sp_Documentos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
