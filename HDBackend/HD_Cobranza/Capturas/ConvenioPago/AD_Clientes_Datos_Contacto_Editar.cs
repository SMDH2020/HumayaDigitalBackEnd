﻿using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class AD_Clientes_Datos_Contacto_Editar
    {
        private string CadenaConexion;
        public AD_Clientes_Datos_Contacto_Editar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Datos_Contacto_Editar>>

            Editar(mdlClientes_Datos_Contacto_Editar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @medio_contacto = mdl.medio_contacto,
                    @orden = mdl.orden,
                    @valor = mdl.valor,
                    @comentarios = mdl.comentarios,
                    @usuario = mdl.usuario,
                };

                var result = await
                factory.SQL.QueryAsync<mdlClientes_Datos_Contacto_Editar>("Credito.Clientes_Datos_Contacto_Cobranza_Editar",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }
    }
}