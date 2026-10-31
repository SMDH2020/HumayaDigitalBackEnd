using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.PartesFamilia;
using Postventa.Modelos.PartesMultiplicador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.PartesFamilia
{
    public class AD_Partes_Familia_Listado
    {
        private string CadenaConexion;
        public AD_Partes_Familia_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Partes_Familia>> Listado()
        {
            try
            {
                var parametros = new DynamicParameters();
                //parametros.Add("folio", folio, System.Data.DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Partes_Familia> result = await factory.SQL.QueryAsync<mdl_Partes_Familia>("EQUIP.Ventas.sp_Familia_Refacciones_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
