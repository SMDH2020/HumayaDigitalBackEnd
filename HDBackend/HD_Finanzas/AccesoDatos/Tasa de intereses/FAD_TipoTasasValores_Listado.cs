using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Tasa_de_intereses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.Tasa_de_intereses
{
    public class FAD_TipoTasasValores_Listado
    {
        private string CadenaConexion;
        public FAD_TipoTasasValores_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<Fmdl_TipoTasasValores>> Listado(int idtipo_tasa)
        {
            try
            {
                var parametros = new
                {
                    idtipo_tasa
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<Fmdl_TipoTasasValores> result = await factory.SQL.QueryAsync<Fmdl_TipoTasasValores>("Credito.sp_Tipo_Tasas_Valores_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
