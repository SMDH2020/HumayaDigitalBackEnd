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
    public class ADCob_Total_Cartera_Detalle
    {
        private string CadenaConexion;
        public ADCob_Total_Cartera_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCob_Total_Cartera_Detalle>> Listado(string adr, string sucursal, int ejercicio, int periodo, string linea, string usuario)
        {
            if(adr == "0")
            {
                adr = "";
            }

            if(sucursal == "0")
            {
                sucursal = "100";
            }
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    region = adr,
                    sucursales = sucursal,
                    lineas = linea,
                    usuario = 1
                };
                var result = await factory.SQL.QueryAsync<mdlCob_Total_Cartera_Detalle>("EQUIP.Credito.sp_Obtener_TotalCartera_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
