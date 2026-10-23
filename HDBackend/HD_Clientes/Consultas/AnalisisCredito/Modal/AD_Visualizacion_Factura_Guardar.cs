using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class AD_Visualizacion_Factura_Guardar
    {
        private string CadenaConexion;
        public AD_Visualizacion_Factura_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> GuardarVisualizacion(mdl_Visualizacion_Factura mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro = mdl.registro,
                    usuario = mdl.usuario

                };
                await factory.SQL.ExecuteAsync("Credito.sp_Solicitud_Credito_Visualizar_Factura_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
