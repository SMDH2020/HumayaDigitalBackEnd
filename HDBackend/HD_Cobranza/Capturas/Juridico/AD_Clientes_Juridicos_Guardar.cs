using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.Juridico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Consultas.Juridico
{
    public class AD_Clientes_Juridicos_Guardar
    {
        private string CadenaConexion;
        public AD_Clientes_Juridicos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Clientes_Juridico_Correo> Guardar(mdl_Clientes_Juridico_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @estatus = mdl.estatus,
                    @comentarios= mdl.comentarios,
                    @detalle = mdl.detalle,
                    @usuario = mdl.usuario,
                };

                mdl_Clientes_Juridico_Correo result = await factory.SQL.QueryFirstAsync<mdl_Clientes_Juridico_Correo>("Cobranza.sp_Clientes_Enviar_Juridico_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
