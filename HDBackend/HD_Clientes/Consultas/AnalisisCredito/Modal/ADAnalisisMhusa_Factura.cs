using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class ADAnalisisMhusa_Factura
    {
        private string CadenaConexion;
        public ADAnalisisMhusa_Factura(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlJDFAnalisis_Datos_Facturacion> Obtener(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Analisis_MHUSA_Datos_Facturacion_View", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlJDFAnalisis_Datos_Facturacion> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    factura = mdl.factura,

                    comentarios = mdl.comentarios,
                    estatus = mdl.estatus,
                    idequip = mdl.idequip,
                    idsucursal = mdl.idsucursal,
                    serie_fiscal = mdl.serie_fiscal,
                    folio_fiscal = mdl.folio_fiscal,
                    documento = mdl.documento,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Analisis_MHUSA_Datos_Facturacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }


    }
}

