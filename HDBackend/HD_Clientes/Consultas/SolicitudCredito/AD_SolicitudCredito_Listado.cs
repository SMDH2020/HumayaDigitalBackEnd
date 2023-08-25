﻿using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolicitudCredito_Listado
    {
        private string CadenaConexion;
        public AD_SolicitudCredito_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitud_Credito>> Listado(short filtrar)
        {
            try
            {
                var parametros = new
                {
                    filtrar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitud_Credito> result = await factory.SQL.QueryAsync<mdlSolicitud_Credito>("Credito.sp_solicitud_credito_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
