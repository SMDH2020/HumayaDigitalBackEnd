using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesDatosPersonaFisica
{
    public class AD_ClientesDatosPersonaFisica_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesDatosPersonaFisica_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlClientes_Datos_Persona_Fisica mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    nombre = mdl.nombre,
                    apellido_paterno = mdl.apellido_paterno,
                    apellido_materno = mdl.apellido_materno,
                    curp = mdl.curp,
                    sexo = mdl.sexo,
                    estado_civil = mdl.estado_civil,
                    regimen_conyugal = mdl.regimen_conyugal,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_clientes_datos_persona_fisica_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
