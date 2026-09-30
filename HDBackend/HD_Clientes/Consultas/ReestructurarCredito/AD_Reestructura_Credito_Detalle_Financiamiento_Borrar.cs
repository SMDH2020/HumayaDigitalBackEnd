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
    public class AD_Reestructura_Credito_Detalle_Financiamiento_Borrar
    {
        private string CadenaConexion;
        public AD_Reestructura_Credito_Detalle_Financiamiento_Borrar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Detalle_Financiamiento_View> Delete(string folio, int docto, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    docto,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Reestructura_Credito_Detalle_Financiamiento_Delete", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPedido_Detalle_Financiamiento_View view = new mdlPedido_Detalle_Financiamiento_View();
                view.info = result.Read<mdlPedido_Detalle_Financiamiento_Info>().FirstOrDefault();
                view.detalle_financiamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlPedido_Detalle_Financiamiento_View> DeleteAll(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.Reestructura_Credito_Detalle_Financiamiento_EliminarAmortizaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPedido_Detalle_Financiamiento_View view = new mdlPedido_Detalle_Financiamiento_View();
                view.info = result.Read<mdlPedido_Detalle_Financiamiento_Info>().FirstOrDefault();
                view.detalle_financiamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
