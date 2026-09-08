using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ClientesEQUIP
{
    public class AD_Clientes_Sucursales_Equip
    {
        private string CadenaConexion;
        public AD_Clientes_Sucursales_Equip(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Sucursal_Equiq>> BuscarID(string idcliente_equip)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idequip=idcliente_equip
                };
                IEnumerable<mdlClientes_Sucursal_Equiq> result = await factory.SQL.QueryAsync<mdlClientes_Sucursal_Equiq>("Credito.sp_Obtener_Sucursal_Cliente_Equip", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
