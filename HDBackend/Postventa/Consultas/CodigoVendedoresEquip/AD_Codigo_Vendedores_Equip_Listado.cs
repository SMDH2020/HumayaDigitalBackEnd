using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using Postventa.Modelos.CodigoVendedoresEquip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.CodigoVendedoresEquip
{
    public class AD_Codigo_Vendedores_Equip_Listado
    {
        private string CadenaConexion;
        public AD_Codigo_Vendedores_Equip_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Codigo_Vendedores_Equip_Listado>> Listado()
        {
            try
            {
                var parametros = new DynamicParameters();
                //parametros.Add("folio", folio, System.Data.DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Codigo_Vendedores_Equip_Listado> result = await factory.SQL.QueryAsync<mdl_Codigo_Vendedores_Equip_Listado>("EQUIP.Ventas.sp_Codigo_Vendedores_Equip_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
