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
        public async Task<IEnumerable<mdlCob_TotalCartera_Detalle>> Listado(int idsucursal,string linea)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    region="",
                    sucursales=idsucursal,
                    lineas= linea,
                    usuario =1
                };
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCartera_Detalle>("Equip.Credito.sp_Obtener_TotalCartera_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlCob_TotalCartera_Detalle> listado = result.ToList();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlCob_TotalCartera_Detalle>> ListadoPorCliente(int cliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    cliente,
                    usuario = 1
                };
                var result = await factory.SQL.QueryAsync<mdlCob_TotalCartera_Detalle>("Equip.Credito.sp_Obtener_TotalCartera_Detalle_Cliente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                List<mdlCob_TotalCartera_Detalle> listado = result.ToList();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
