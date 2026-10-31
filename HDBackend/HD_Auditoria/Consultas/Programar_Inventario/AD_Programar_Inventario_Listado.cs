using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Programar_Inventario
{
    public class AD_Programar_Inventario_Listado
    {
        private string CadenaConexion;
        public AD_Programar_Inventario_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Programar_Inventario_Listado_View> Listado(string? usuario)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@usuario", usuario, System.Data.DbType.String, System.Data.ParameterDirection.Input);


                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryMultipleAsync("Auditoria.SP_PROG_AUDITORIA_LISTADO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Programar_Inventario_Listado_View listado = new mdl_Programar_Inventario_Listado_View();
                listado.inventario = result.Read<mdl_Listado_Inventario>().ToList();
                listado.responsable = result.Read<mdl_Responsable_Almacen>().ToList();
                listado.categorias = result.Read<mdl_Categorias>().ToList();
                listado.sucursales = result.Read<mdl_Sucursales>().ToList();
                listado.auditores = result.Read<mdl_Usuarios>().ToList();
                listado.empleados = result.Read<mdl_Usuarios>().ToList();
                factory.SQL.Close();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
