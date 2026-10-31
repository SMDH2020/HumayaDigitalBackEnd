using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_Vendedor_Guardar
    {
        private string CadenaConexion;
        public AD_Clientes_Vendedor_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Vendedor> Guardar_Persona_Moral(mdlClientes mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    rfc = mdl.rfc,
                    razon_social = mdl.razon_social,
                    tipo_persona = mdl.tipo_persona,
                    medio_contacto = mdl.medio_contacto,
                    tiempo_agricultor = mdl.tiempo_agricultor,
                    agrupacion = mdl.agrupacion,
                    regimen_fiscal = mdl.regimen_fiscal,
                    idvendedor = mdl.idvendedor,
                    tipo_venta = mdl.tipo_venta,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Vendedor>("Credito.sp_clientes_Vendedor_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
