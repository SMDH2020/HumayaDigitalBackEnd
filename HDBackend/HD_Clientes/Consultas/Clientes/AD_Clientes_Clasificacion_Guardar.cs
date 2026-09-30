using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_Clasificacion_Guardar
    {
        private string CadenaConexion;
        public AD_Clientes_Clasificacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<int> GuardarClasificacion(mdl_Cliente_Clasificacion mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    clasificacion = mdl.clasificacion,
                    usuario = mdl.usuario
                };
                mdl.idcliente = await factory.SQL.QueryFirstAsync<int>("Credito.sp_Clientes_Clasificacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return mdl.idcliente;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
