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
    public class FAD_TipoTasas_ListadoporAnio
    {
        private string CadenaConexion;
        public FAD_TipoTasas_ListadoporAnio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<Fmdl_TipoTasas>> ListadoAnio(int anio)
        {
            try
            {
                var parametros = new
                {
                    anio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<Fmdl_TipoTasas> result = await factory.SQL.QueryAsync<Fmdl_TipoTasas>("Credito.sp_Tipo_Tasas_ListadoporAnio", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
