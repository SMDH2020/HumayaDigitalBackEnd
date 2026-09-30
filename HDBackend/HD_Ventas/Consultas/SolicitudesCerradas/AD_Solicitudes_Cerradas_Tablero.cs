using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.SolicitudesCerradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.SolicitudesCerradas
{
    public class AD_Solicitudes_Cerradas_Tablero
    {
        private string CadenaConexion;
        public AD_Solicitudes_Cerradas_Tablero(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Listado_Solicitudes_Cerradas_View> obtenerTablero(string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario
                };
                IEnumerable<mdl_Solicitudes_Tablero> tablero = await factory.SQL.QueryAsync<mdl_Solicitudes_Tablero>("Ventas.sp_Solicitud_Credito_Cerradas_Tablas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdl_Solicitudes_Vendedor>? vendedor = tablero.GroupBy(item => item.idvendedor).Select(element => new mdl_Solicitudes_Vendedor { idvendedor = element.First().idvendedor, vendedor = element.First().vendedor, idsucursal = element.First().idsucursal }).ToList();
                List<mdl_Solicitudes_Sucursal> sucursal = tablero.GroupBy(item => item.idsucursal).Select(element => new mdl_Solicitudes_Sucursal { idsucursal = element.First().idsucursal, sucursal = element.First().sucursal }).ToList();

                mdl_Listado_Solicitudes_Cerradas_View view = new mdl_Listado_Solicitudes_Cerradas_View();
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
