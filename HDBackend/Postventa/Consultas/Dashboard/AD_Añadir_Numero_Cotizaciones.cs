using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Añadir_Numero_Cotizaciones
    {
        private string CadenaConexion;
        public AD_Añadir_Numero_Cotizaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> AgregarContactoGarantias(mdl_Agregar_Contacto_Cotizaciones mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_garantia = mdl.id_garantia,
                    contacto = mdl.contacto,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Postventa.sp_Añadir_Contacto_Garantias", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Agregar_Contacto_Cotizaciones> GetTelefonoGarantias(int idgarantia)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("id_garantia", idgarantia, System.Data.DbType.Int16);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Agregar_Contacto_Cotizaciones result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Agregar_Contacto_Cotizaciones>("Postventa.sp_Obtener_Contacto_Garantias", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
