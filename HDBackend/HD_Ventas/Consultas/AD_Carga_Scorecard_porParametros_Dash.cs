﻿using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;


namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_porParametros_Dash
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_porParametros_Dash(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash>> Scorecard(int region, int sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    region = region,
                    sucursal = sucursal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_porVendedor_Dash> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_porVendedor_Dash>("Ventas.Obtener_Scorecard_porParametro", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
