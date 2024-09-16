using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas.Modelos;

namespace Ventas.Consultas
{
    public class AD_DDLVendedor
    {
        private string CadenaConexion;
        public AD_DDLVendedor(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_ddlVendedor>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                IEnumerable<mdl_ddlVendedor> result = await factory.SQL.QueryAsync<mdl_ddlVendedor>("Credito.sp_Vendedores_Listado", commandType: System.Data.CommandType.StoredProcedure);
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
