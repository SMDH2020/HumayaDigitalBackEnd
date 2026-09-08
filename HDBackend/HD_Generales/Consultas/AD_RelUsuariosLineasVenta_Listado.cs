using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Consultas
{
    public class AD_RelUsuariosLineasVenta_Listado
    {
        private string CadenaConexion;
        public AD_RelUsuariosLineasVenta_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Rel_Usuario_Lineas_Venta>> ListadoLineasVentaRel(int idlinea, int idusuario)
        {
            try
            {
                var parametros = new
                {
                    idlinea,
                    idusuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Rel_Usuario_Lineas_Venta> result = await factory.SQL.QueryAsync<mdl_Rel_Usuario_Lineas_Venta>("humayadigital_usuarios.dbo.sp_Usuarios_Rel_Usuarios_Lineas_Venta_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
