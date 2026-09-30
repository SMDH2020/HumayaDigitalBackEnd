using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.CodigoVendedoresEquip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.CodigoVendedoresEquip
{
    public class AD_Codigo_Vendedores_Equip_Guardar
    {
        private string CadenaConexion;
        public AD_Codigo_Vendedores_Equip_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_Codigo_Vendedores_Equip_Guardar mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("idlistado", mdl.idlistado, System.Data.DbType.Int32);
                parametros.Add("codigo", mdl.codigo, System.Data.DbType.String);
                parametros.Add("vendedor", mdl.vendedor, System.Data.DbType.String);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("EQUIP.Ventas.sp_Codigo_Vendedores_Equip_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
