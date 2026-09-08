using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.CodigoVendedoresEquip;
using Postventa.Modelos.PartesMultiplicador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.PartesMultiplicador
{
    public class AD_Partes_Multiplicador_Listado_Partes
    {
        private string CadenaConexion;
        public AD_Partes_Multiplicador_Listado_Partes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<string>> ListadoPartes()
        {
            try
            {
                var parametros = new DynamicParameters();
                //parametros.Add("folio", folio, System.Data.DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<string> result = await factory.SQL.QueryAsync<string>("EQUIP.Ventas.sp_Partes_Multiplicador_Piezas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
