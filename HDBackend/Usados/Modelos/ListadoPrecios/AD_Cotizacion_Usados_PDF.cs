using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Inventario;
using Usados.Consultas.Usados;

namespace Usados.Modelos.ListadoPrecios
{
    public class AD_Cotizacion_Usados_PDF
    {
        private string CadenaConexion;
        public AD_Cotizacion_Usados_PDF(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Cotizacion_Usados_PDF>> Cotizacion(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cotizacion_Usados_PDF> result = await factory.SQL.QueryAsync<mdl_Cotizacion_Usados_PDF>("Usados.sp_Obtener_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
