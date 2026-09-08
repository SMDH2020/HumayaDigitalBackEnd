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
    public class AD_Codigo_Vendedores_Equip_Codigos
    {
        private string CadenaConexion;
        public AD_Codigo_Vendedores_Equip_Codigos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Codigos_View> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new DynamicParameters();
                //parametros.Add("ejercicio_inicio", ejercicio_inicio, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.Ventas.sp_Codigo_Vendedores_Equip_Codigos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Codigos_View();
                view.codigo = result.Read<string>().ToList();
                view.vendedores = result.Read<string>().ToList();

                //view.tipo_cartera = result.Read<string>().FirstOrDefault();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
