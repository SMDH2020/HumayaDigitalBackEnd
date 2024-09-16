﻿using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas.ComentariosClientesContacto
{
    public class AD_Guarda_Comentarios_ClientesContacto
    {
        private string CadenaConexion;
        public AD_Guarda_Comentarios_ClientesContacto(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlComentarios_Clientes_Contacto>> Comentario(int idcliente, string comentario, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                    @comentario = comentario,
                    @usuario = usuario
                };
                IEnumerable<mdlComentarios_Clientes_Contacto> result = await factory.SQL.QueryAsync<mdlComentarios_Clientes_Contacto>("Cobranza.sp_Guarda_Comentarios_ClienteContacto", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
