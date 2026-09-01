using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.RelacionDocumentosSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.RelacionDocumentosSC
{
    public class AD_RelacionDocumentosSC_Listado
    {
        private string CadenaConexion;
        public AD_RelacionDocumentosSC_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_RelacionDocumentosSC_Listado_View> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Rel_Documentos_SC_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_RelacionDocumentosSC_Listado_View view = new mdl_RelacionDocumentosSC_Listado_View();
                view.Listado = result.Read<mdl_RelacionDocumentosSC_Listado>().ToList();
                view.DocumentosMhusa = result.Read<mdl_Documentos_Listado>().ToList();
                view.DocumentosJDF = result.Read<mdl_Documentos_Listado>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
