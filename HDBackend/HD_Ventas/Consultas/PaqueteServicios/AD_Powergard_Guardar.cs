using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PaqueteServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Powergard_Guardar
    {
        private string CadenaConexion;
        public AD_Powergard_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> Guardar(mdl_Powergard_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idpowergard = mdl.idpowergard,
                    idsucursal = mdl.idsucursal,
                    cliente = mdl.idcliente,
                    serie = mdl.serie,
                    facturacion = mdl.facturacion,
                    costo = mdl.costo,
                    tipo = mdl.tipo,
                    fecha_facturacion = mdl.fecha_facturacion ,
                    num_ot = mdl.num_ot ,
                    vendedor = mdl.idvendedor ,
                    cobertura = mdl.cobertura ,
                    contrato = mdl.contrato,
                    usuario = mdl.usuario 
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Powergard_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
