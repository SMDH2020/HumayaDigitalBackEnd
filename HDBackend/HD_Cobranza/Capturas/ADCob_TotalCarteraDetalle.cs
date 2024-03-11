using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas
{
    public  class ADCob_TotalCarteraDetalle
    {
        private string CadenaConexion;
        public ADCob_TotalCarteraDetalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_TotalCartera_Detalle>> Listado(int idsucursal,string linea,string? usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idsucursal,
                    linea,
                    usuario 
                };
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCartera_Detalle>("Credito.sp_Obtener_resumen_cartera_por_cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                //List<mdlCob_TotalCartera_Detalle> listado = result.ToList();
                List<mdlCob_TotalCartera_Detalle> listado = result.ToList();
                if(result.Count()>0)
                listado.Add(new mdlCob_TotalCartera_Detalle()
                {
                    idsucursal = result.First().idsucursal,
                    sucursal = result.First().sucursal,
                    departamento = "TOTAL",
                    mas90 = result.Sum(x => x.mas90),
                    mas60 = result.Sum(x => x.mas60),
                    mas30 = result.Sum(x => x.mas30),
                    mas15 = result.Sum(x => x.mas15),
                    de1a15 = result.Sum(x => x.de1a15),
                    vencido = result.Sum(x => x.vencido),
                    porvencer = result.Sum(x => x.porvencer),
                    totalcartera = result.Sum(x => x.totalcartera),
                    saldoafavor = result.Sum(x => x.saldoafavor),
                    total = result.Sum(x => x.total),
                    activo = result.Sum(x => x.activo),
                    juridico = result.Sum(x => x.juridico),
                });                
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlResumenCartera_Clientes>> ListadoPorCliente(int cliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    cliente,
                    usuario = 1
                };
                var result = await factory.SQL.QueryAsync<mdlResumenCartera_Clientes>("Equip.Credito.sp_Obtener_TotalCartera_Detalle_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlResumenCartera_Clientes> listado = result.ToList();
                if (result.Count() > 0)
                    listado.Add(new mdlResumenCartera_Clientes()
                    {
                        //idsucursal = result.First().idsucursal,
                        linea = result.First().linea,
                        //departamento = "TOTAL",
                        sucursal = result.First().sucursal,
                        documento = result.First().documento,
                        vencimiento = result.First().vencimiento,
                        diasvencido = result.First().diasvencido,
                        saldo = result.Sum(x => x.saldo),
                        interesbase = result.Sum(x => x.interesbase),
                        importe = result.Sum(x => x.importe),
                    });
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
