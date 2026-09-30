using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Buro.Consultas
{
    public class AD_Cartera_Vencida_Guardar
    {
        private string CadenaConexion;
        public AD_Cartera_Vencida_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCartera_Vencida>> Guardar(mdlCartera_Vencida modelo)
        {
            try
            {

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id=modelo.id,
                    cliente = modelo.cliente,
                    fecha = modelo.fecha,
                    telefono = modelo.telefono,
                    telefonocel = modelo.telefonoCel,
                    sucursal = modelo.sucursal,
                    nombresucursal = modelo.nombreSucursal,
                    nombre = modelo.nombre,
                    valororiginal = modelo.valororiginal,
                    reg = modelo.reg,
                    pagado = modelo.pagado,
                    saldo = modelo.saldo,
                    credito = modelo.credito,
                    terminocred2 = modelo.terminocred2,
                    terminocred1 = modelo.terminocred1,
                    tipoclave = modelo.tipoclave,
                    invo = modelo.invo,
                    origen = modelo.origen,
                    nombremodulo = modelo.nombremodulo,
                    seriefiscal = modelo.seriefiscal,
                    docfiscal = modelo.docfiscal,
                    nombremodulo2 = modelo.nombremodulo2,
                    terminocredX = modelo.terminocredX,
                    terminocred = modelo.terminocred,
                    vencimiento = modelo.vencimiento,
                    mas360 = modelo.mas360,
                    de271a360 = modelo.de271a360,
                    de211a270 = modelo.de211a270,
                    de151a210 = modelo.de151a210,
                    de91a150 = modelo.de91a150,
                    de61a90 = modelo.de61a90,
                    de31a60 = modelo.de31a60,
                    de16a30 = modelo.de16a30,
                    de1a15 = modelo.de1a15,
                    porvencer = modelo.porvencer,
                    totalvencido = modelo.totalvencido,
                    subtotal = modelo.subtotal,
                    total = modelo.total,
                    usuario = modelo.usuario
                };
               var result = await factory.SQL.QueryAsync<mdlCartera_Vencida>("BuroCredito.dbo.sp_Cartera_Vencida_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
