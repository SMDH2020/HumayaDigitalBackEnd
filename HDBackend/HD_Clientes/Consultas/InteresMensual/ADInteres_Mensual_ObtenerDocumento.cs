using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.InteresMensual
{
    public class ADInteres_Mensual_ObtenerDocumento
    {
        private string CadenaConexion;
        public ADInteres_Mensual_ObtenerDocumento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInteres_Mensual> Obtener(int idinteres)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinteres
                };
                mdlInteres_Mensual result = await factory.SQL.QueryFirstOrDefaultAsync<mdlInteres_Mensual>("Credito.sp_Interes_Mensual_ObtenerDocumento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
