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
    public class FAD_TipoTasas_ObtenerporID
    {
        private string CadenaConexion;
        public FAD_TipoTasas_ObtenerporID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<Fmdl_TipoTasas> BuscarID(int idtipo_tasa)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idtipo_tasa
                };
                Fmdl_TipoTasas result = await factory.SQL.QueryFirstOrDefaultAsync<Fmdl_TipoTasas>("Credito.sp_Tipo_Tasas_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
