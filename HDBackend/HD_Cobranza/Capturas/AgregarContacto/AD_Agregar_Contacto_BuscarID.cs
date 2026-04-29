using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.AgregarContacto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.AgregarContacto
{
    public class AD_Agregar_Contacto_BuscarID
    {

        private string CadenaConexion;
        public AD_Agregar_Contacto_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAgregarContacto> Obtener(int idmedio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idmedio
                };
                mdlAgregarContacto result = await factory.SQL.QueryFirstOrDefaultAsync<mdlAgregarContacto>("Cobranza.sp_Clientes_Contacto_BuscarID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
