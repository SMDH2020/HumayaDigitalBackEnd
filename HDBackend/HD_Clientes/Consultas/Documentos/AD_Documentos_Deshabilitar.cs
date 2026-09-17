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
    public class AD_Documentos_Deshabilitar
    {
        private string CadenaConexion;
        public AD_Documentos_Deshabilitar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDocumentos>> borrar(int iddocumento, int jdf)
        {
            try
            {
                var parametros = new
                {
                    iddocumento,
                    jdf
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlDocumentos> result = await factory.SQL.QueryAsync<mdlDocumentos>("Credito.sp_Documentos_Deshabilitar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
