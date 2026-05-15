using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Conteo_Piezas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Programar_Inventario
{
    public class AD_ProgramarInventario_IniciarConteo
    {
        private string CadenaConexion;
        public AD_ProgramarInventario_IniciarConteo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> folio(string folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("folio", folio, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("Auditoria.SP_PROG_AUDITORIA_INIIAR_CONTEO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
