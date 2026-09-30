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
    public class AD_Piezas_Multiplicador_Guardar
    {
        private string CadenaConexion;
        public AD_Piezas_Multiplicador_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_Partes mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("idparte", mdl.idparte, System.Data.DbType.Int32);
                parametros.Add("parte", mdl.parte, System.Data.DbType.String);
                parametros.Add("multiplicador", mdl.multiplicador, System.Data.DbType.Double);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("EQUIP.Ventas.sp_Partes_Multiplicador_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
