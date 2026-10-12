using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.RelacionDocumentosSC
{
    public class AD_RelacionDocumentosSC_Delete
    {
        private string CadenaConexion;
        public AD_RelacionDocumentosSC_Delete(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> borrar(int idmhusa, int idJDF)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    iddocumentoJDF = idJDF,
                    iddocumentoMhusa = idmhusa
                };
                await factory.SQL.QueryAsync("Credito.sp_Rel_Documentos_SC_Borrar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
