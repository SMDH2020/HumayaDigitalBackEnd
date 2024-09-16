using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas
{
    public class ADRegistroClientes
    {
        private string CadenaConexion;
        public ADRegistroClientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<int> Guardar(ctlRegistrarCliente mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    rfc=mdl.rfc,
                    idequip=mdl.idequip,
                    razon_social=mdl.razon_social,
                    usuario=mdl.usuario,
                };
                int result = await factory.SQL.QueryFirstOrDefaultAsync<int>("Cobranza.sp_cliente_Registrar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<int> Combinar(ctlCombinarCliente mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente=mdl.idcliente,
                    idequip = mdl.idequip,
                    usuario = mdl.usuario,
                };
                int result = await factory.SQL.QueryFirstOrDefaultAsync<int>("Credito.sp_Cliente_Combinar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
