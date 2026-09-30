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
    public class AD_Partes_Familia_Guardar
    {
        private string CadenaConexion;
        public AD_Partes_Familia_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_Partes_Familia mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("numero_parte", mdl.numero_parte, System.Data.DbType.String);
                parametros.Add("nombre_parte", mdl.nombre_parte, System.Data.DbType.String);
                parametros.Add("linea", mdl.linea, System.Data.DbType.String);
                parametros.Add("familia", mdl.familia, System.Data.DbType.String);
                parametros.Add("subfamilia_1", mdl.subfamilia_1, System.Data.DbType.String);
                parametros.Add("subfamilia_2", mdl.subfamilia_2, System.Data.DbType.String);
                parametros.Add("usuario", mdl.usuario, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("EQUIP.Ventas.sp_Familia_Refacciones_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
