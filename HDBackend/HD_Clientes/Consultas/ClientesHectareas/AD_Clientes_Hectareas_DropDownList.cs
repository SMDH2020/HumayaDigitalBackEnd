﻿using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes_Hectareas
{
    public class AD_Clientes_Hectareas_DropDownList
    {
        private string CadenaConexion;
        public AD_Clientes_Hectareas_DropDownList(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Hectareas>> DropDownList()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_Hectareas> result = await factory.SQL.QueryAsync<mdlClientes_Hectareas>("sp_clientes_hectareas_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
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
