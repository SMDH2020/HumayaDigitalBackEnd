using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.RelacionDocumentosSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.RelacionDocumentosSC
{
    public class AD_RelacionDocumentosSC_Guardar
    {
        private string CadenaConexion;
        public AD_RelacionDocumentosSC_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_RelacionDocumentosSC_Guardar mdl, string usuario)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    detalle = mdl.detalle,
                    usuario = usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Rel_Documentos_SC_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
