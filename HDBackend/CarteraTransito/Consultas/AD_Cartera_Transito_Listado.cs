using CarteraTransito.Modelos;
using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteraTransito.Consultas
{
    public class AD_Cartera_Transito_Listado
    {
        private string CadenaConexion;
        public AD_Cartera_Transito_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Cartera_Transito>> Listado(int ejercicio, int periodo, string? sucursal, string? adr)
        {
            try
            {
                var parametros = new
                {
                    @ejercicio = ejercicio,
                    @periodo = periodo,
                    sucursal = sucursal,
                    adr = adr
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cartera_Transito> result = await factory.SQL.QueryAsync<mdl_Cartera_Transito>("Cartera_Clientes.dbo.sp_Cartera_Transito_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
