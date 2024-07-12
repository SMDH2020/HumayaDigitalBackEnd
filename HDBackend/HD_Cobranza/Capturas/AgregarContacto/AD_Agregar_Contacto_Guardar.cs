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
    public class AD_Agregar_Contacto_Guardar
    {

        private string CadenaConexion;
        public AD_Agregar_Contacto_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlAgregarContacto>> Guardar(mdlAgregarContacto mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idmedio = mdl.idmedio,
                    @idcliente = mdl.idcliente,
                    @mediocontacto = mdl.mediocontacto,
                    @medio = mdl.medio,
                    @comentarios = mdl.comentarios,
                    @usuario = mdl.usuario,
                };

                var result = await factory.SQL.QueryAsync<mdlAgregarContacto>("Cobranza.sp_Clientes_Contacto_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
