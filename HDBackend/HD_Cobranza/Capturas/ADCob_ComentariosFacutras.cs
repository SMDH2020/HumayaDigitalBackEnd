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
    public class ADCob_ComentariosFacutras
    {
        private string CadenaConexion;
        public ADCob_ComentariosFacutras(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlCob_ComentariosFactura obj)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcomentario = obj.idcomentario,
                    @idcliente = obj.idcliente,
                    @comentarios = obj.comentarios,
                    @formacontacto = obj.formacontacto,
                    @compromisopago = obj.compromisopago,
                    @fechacompromisopago = obj.fechacompromisopago,
                    @importeconvenio = obj.importeconvenio,
                    @recordatorio = obj.recordatorio,
                    @fecharecordatorio = obj.fecharecordatorio,
                    @usuario = obj.usuario,
                };

                await factory.SQL.QueryAsync("Credito.SP_Comentarios_Factura_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlCob_ComentariosFactura>> Listado(int idcliente,int idfactura)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente,
                    idfactura
                };
                var result = await factory.SQL.QueryAsync<mdlCob_ComentariosFactura>("Credito.sp_Comentarios_Factura_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
