using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_CentroMonitoreo.Modelos;

namespace HD_CentroMonitoreo.Consultas.Organizacion
{
    public class AD_Organizacion
    {
        private string CadenaConexion;

        public AD_Organizacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<List<mdl_Organizacion>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryAsync<mdl_Organizacion>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Organizacion_Listado",
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Organizacion> Detalle(string jd_org_id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    jd_org_id = jd_org_id
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Organizacion>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Organizacion_Detalle",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }
    }
}