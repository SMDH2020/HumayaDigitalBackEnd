using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.SolicitudesCerradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas
{
    public class AD_Solicitudes_Facturadas_Excel
    {
        private string CadenaConexion;
        public AD_Solicitudes_Facturadas_Excel(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlOperacionesDetalle>> Listado(int ejercicio, int periodo, string linea)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio,
                    periodo,
                    linea
                };
                var result = await factory.SQL.QueryAsync<mdlOperacionesDetalle>("Ventas.sp_Solicitudes_Credito_Facturadas_Detalle_Excel", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                List<mdlOperacionesDetalle> listado = result.ToList();
                //listado.Add(new mdlOperacionesDetalle()
                //{
                //    idsucursal = 13,
                //    sucursal = "TOTALES",
                //    mas90 = result.Sum(x => x.mas90),
                //    mas60 = result.Sum(x => x.mas60),
                //    mas30 = result.Sum(x => x.mas30),
                //    mas15 = result.Sum(x => x.mas15),
                //    de1a15 = result.Sum(x => x.de1a15),
                //    vencido = result.Sum(x => x.vencido),
                //    activo = result.Sum(x => x.activo),
                //    porvencer = result.Sum(x => x.porvencer),
                //    totalcartera = result.Sum(x => x.totalcartera),
                //    saldoafavor = result.Sum(x => x.saldoafavor),
                //    total = result.Sum(x => x.total),
                //    juridico = result.Sum(x => x.juridico),
                //});

                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
