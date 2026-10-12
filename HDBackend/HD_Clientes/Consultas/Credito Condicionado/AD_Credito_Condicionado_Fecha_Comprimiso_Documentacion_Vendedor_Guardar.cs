using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar
    {

        private string CadenaConexion;
        FactoryConection factory;
        public AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
            factory= new FactoryConection(CadenaConexion);
        }
        public void Commit()
        {       
            factory.SQL.Close();
        }
        public async Task<bool> Guardar(mdl_fecha_compromiso_documentos mdl)
        {
            try
            {
                var parametros = new
                {
                    @folio = mdl.folio,
                    @usuario = mdl.usuario,
                    @fecha_compromiso = mdl.fecha_compromiso, 
                    @comentarios = mdl.comentarios,
                    @enviar_revision = mdl.enviar_revision,
                    @iddocumento = mdl.iddocumento,
                };
                await factory.SQL.QueryAsync("Credito.sp_Solicitud_Credito_Documentacion_Fecha_Compromiso_Vendedor_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
