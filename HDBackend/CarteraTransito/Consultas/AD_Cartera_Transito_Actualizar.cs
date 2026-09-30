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
    public class AD_Cartera_Transito_Actualizar
    {
        private string CadenaConexion;
        public AD_Cartera_Transito_Actualizar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> actualizar(int ejercicio, int periodo, int ejerciciotransito, int periodotransito)
        {
            try
            {
                var parametros = new
                {
                    @ejercicio = ejercicio,
                    @periodo = periodo,
                    @ejercicio_transito = ejerciciotransito,
                    @periodo_transito = periodotransito
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Cartera_Clientes.dbo.sp_Cargar_Facturas_Transito", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
