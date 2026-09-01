using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReestructurarCredito
{
    public class AD_Reestructura_Credito_Condiciones_Credito_Guardar
    {
        private string CadenaConexion;
        public AD_Reestructura_Credito_Condiciones_Credito_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlPedido_Condiciones_Venta mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    condiciones = mdl.condiciones,
                    observaciones = mdl.observaciones,
                    deposito = mdl.deposito,
                    taza = mdl.taza,
                    anticipo = mdl.anticipo,
                    plazo = mdl.plazo,
                    tiempo_plazo = mdl.tiempo_plazo,
                    mhusajdf = mdl.mhusajdf,
                    gastos = mdl.gastos,
                    enganche = mdl.enganche,
                    monto=mdl.monto,
                    moneda = mdl.moneda,
                    fecha_pagare = mdl.fecha_pagare,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Restructura_Credito_Condiciones_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
