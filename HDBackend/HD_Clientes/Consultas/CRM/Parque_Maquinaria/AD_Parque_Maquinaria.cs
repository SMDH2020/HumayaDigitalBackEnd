using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Modelos.CRM.Parque_Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Parque_Maquinaria
{
    public class AD_Parque_Maquinaria
    {
        private string CadenaConexion;
        public AD_Parque_Maquinaria(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Parque_MaquinariaCRM>> Listado(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Parque_MaquinariaCRM> result = await factory.SQL.QueryAsync<mdl_Listado_Parque_MaquinariaCRM>("CRM.sp_Obtener_Listado_Parque_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
