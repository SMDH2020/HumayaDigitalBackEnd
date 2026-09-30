using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCredito_Analisis
{
    public class AD_SCAnalisis_Tablero_Mes
    {
        private string CadenaConexion;
        public AD_SCAnalisis_Tablero_Mes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_View> Get(string usuario, int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario,
                    ejercicio,
                    periodo
                };
                IEnumerable<mdlSCAnalisis_Tablero> tablero = await factory.SQL.QueryAsync<mdlSCAnalisis_Tablero>("Credito.sp_Solicitud_Credito_Tablas_Mes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlSCAnalisis_Vendedor>? vendedor = tablero.GroupBy(item => item.idvendedor).Select(element => new mdlSCAnalisis_Vendedor { idvendedor = element.First().idvendedor, vendedor = element.First().vendedor, idsucursal = element.First().idsucursal }).ToList();
                List<mdlSCAnalisis_Sucursal> sucursal = tablero.GroupBy(item => item.idsucursal).Select(element => new mdlSCAnalisis_Sucursal { idsucursal = element.First().idsucursal, sucursal = element.First().sucursal }).ToList();

                mdlSCAnalisis_View view = new mdlSCAnalisis_View();
                view.tablero = tablero;
                view.vendedor = vendedor;
                view.sucursal = sucursal;

                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
