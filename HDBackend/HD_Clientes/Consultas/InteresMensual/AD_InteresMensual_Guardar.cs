using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Consultas.ClientesDatosPersonaFisica;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.InteresMensual
{
    public class AD_InteresMensual_Guardar
    {
        private string CadenaConexion;
        public AD_InteresMensual_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlInteres_Mensual mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idinteres = mdl.idinteres,
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    interes = mdl.interes,
                    documento = mdl.documento,
                    extension = mdl.extension,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Interes_Mensual_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
      
    }
}
