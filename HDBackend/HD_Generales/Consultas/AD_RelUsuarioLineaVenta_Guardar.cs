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
    public class AD_RelUsuarioLineaVenta_Guardar
    {
        private string CadenaConexion;
        public AD_RelUsuarioLineaVenta_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Linea_Venta_Usuario_Permiso>> Guardar(mdl_Linea_Venta_Usuario_Permiso mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idrel = mdl.idrel,
                    @idusuario = mdl.idusuario,
                    @idlinea = mdl.idlinea,
                    @estatus = mdl.estatus,
                    @usuario = mdl.usuario
                };

                var result = await factory.SQL.QueryAsync<mdl_Linea_Venta_Usuario_Permiso>("humayadigital_usuarios.dbo.sp_Rel_Usuarios_Lineas_Venta_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
