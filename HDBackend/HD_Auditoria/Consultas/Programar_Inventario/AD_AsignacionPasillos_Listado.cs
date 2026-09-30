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
    public class AD_AsignacionPasillos_Listado
    {
        private string CadenaConexion;
        public AD_AsignacionPasillos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_ProgramarPasillos_View> Listado(string? folio)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);


                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryMultipleAsync("Auditoria.SP_PROG_AUDITORIA_ASIGNACION_PASILLOS_LISTADO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_ProgramarPasillos_View listado = new mdl_ProgramarPasillos_View();
                listado.auditores = result.Read<mdl_Usuarios>().ToList();
                listado.pasillos = result.Read<mdl_Pasillos>().ToList();
                listado.asignaciones = result.Read<mdl_Pasillos>().ToList();
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
