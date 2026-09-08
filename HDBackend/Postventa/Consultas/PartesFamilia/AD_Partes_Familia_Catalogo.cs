using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos.CodigoVendedoresEquip;
using Postventa.Modelos.PartesFamilia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.PartesFamilia
{
    public class AD_Partes_Familia_Catalogo
    {
        private string CadenaConexion;
        public AD_Partes_Familia_Catalogo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Partes_Familia_Catalogo_View> Catalogo()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new DynamicParameters();
                //parametros.Add("ejercicio_inicio", ejercicio_inicio, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.Ventas.sp_Familia_Refacciones_Catalogo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Partes_Familia_Catalogo_View();
                view.partes = result.Read<mdl_Partes_Familia>().ToList();
                view.familias = result.Read<mdl_Familias>().ToList();
                view.subfamilias1 = result.Read<mdl_Familias>().ToList();
                view.subfamilias2 = result.Read<mdl_Familias>().ToList();

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
