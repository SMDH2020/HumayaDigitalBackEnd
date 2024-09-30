using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Enviar
    {
        private string CadenaConexion;
        public AD_Credito_Condicionado_Enviar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCTimeline_View> BuscarFolio(mdlSCCredito_Condicionado mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio=mdl.folio,
                    usuario=mdl.usuario,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Crear_Solicitud_Credito_Condicionado_JDF", parametros, commandType: System.Data.CommandType.StoredProcedure);

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
        public async Task<bool> Cancelar(mdlSCCredito_Condicionado mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    usuario = mdl.usuario,
                };
               await factory.SQL.QueryMultipleAsync("Credito.sp_TimelineCondicionado_Cancelar", parametros, commandType: System.Data.CommandType.StoredProcedure);



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
