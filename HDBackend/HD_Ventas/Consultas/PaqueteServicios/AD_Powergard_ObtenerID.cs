using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PaqueteServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Powergard_ObtenerID
    {
        private string CadenaConexion;
        public AD_Powergard_ObtenerID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Powergard_Listado> obtener(int idpowergard)
        {
            try
            {
                var parametros = new
                {
                    idpowergard = idpowergard,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Powergard_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Powergard_Listado>("Ventas.sp_Powergard_ObtenerID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
