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
    public class AD_Programar_Inventario_Auditor_Listado
    {
        private string CadenaConexion;
        public AD_Programar_Inventario_Auditor_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Auditores_Listado> buscarFolio(string? folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@folio", folio, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                FactoryConection factory = new FactoryConection(CadenaConexion);

                mdl_Auditores_Listado result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Auditores_Listado>("Auditoria.SP_PROG_AUDITORIA_AUDITORES_LISTADO", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
