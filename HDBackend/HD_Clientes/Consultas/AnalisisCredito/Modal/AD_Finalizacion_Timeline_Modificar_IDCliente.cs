using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class AD_Finalizacion_Timeline_Modificar_IDCliente
    {
        private string CadenaConexion;
        public AD_Finalizacion_Timeline_Modificar_IDCliente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> modificarIDCliente(int idcliente, string? folio, string? usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    idcliente,
                    usuario
                };
                await factory.SQL.ExecuteAsync("Credito.sp_Analisis_Decicion_Finalizacion_ModificarCliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
