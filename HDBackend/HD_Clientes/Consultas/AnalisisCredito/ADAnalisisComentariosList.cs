using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisisComentariosList
    {
        private string CadenaConexion;
        public ADAnalisisComentariosList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlComentariosView>> Listado(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio 
                };
                IEnumerable<mdlComentariosView> result = await factory.SQL.QueryAsync<mdlComentariosView>("Credito.sp_Solicitud_Credito_Comentarios_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
