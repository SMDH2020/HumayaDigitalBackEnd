using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Cargar_Credito_Condicionado
    {
        private string CadenaConexion;
        public AD_Cargar_Credito_Condicionado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCTimeline_View> BuscarFolio(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Timeline_Condicionado", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdlSCTimeline_View view = new mdlSCTimeline_View();
                view.estado = result.Read<mdlSCTimeline_estado>().FirstOrDefault();
                view.detalle = result.Read<mdlSCTimeline_detalle>().ToList();
                if (view.estado is null) view.estado = new mdlSCTimeline_estado();

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
