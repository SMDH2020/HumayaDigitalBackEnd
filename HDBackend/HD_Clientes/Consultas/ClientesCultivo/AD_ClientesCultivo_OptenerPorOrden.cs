﻿using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesCultivo
{
    public class AD_ClientesCultivo_OptenerPorOrden
    {
        private string CadenaConexion;
        public AD_ClientesCultivo_OptenerPorOrden(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlClientes_Cultivo> Get(int idcliente, int registro)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    registro
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdlClientes_Cultivo result = await factory.SQL.QueryFirstOrDefaultAsync<mdlClientes_Cultivo>("Credito.sp_Clientes_Cultivo_ObtenerPorOrden", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
