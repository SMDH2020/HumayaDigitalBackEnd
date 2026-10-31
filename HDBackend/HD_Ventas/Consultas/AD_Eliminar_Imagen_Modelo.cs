using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Eliminar_Imagen_Modelo
    {
        private string CadenaConexion;
        public AD_Eliminar_Imagen_Modelo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> Eliminar(int idmodelo, int numero)
        {
            try
            {
                var parametros = new
                {
                    idmodelo = idmodelo,
                    numero = numero
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Eliminar_Fotografias_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
