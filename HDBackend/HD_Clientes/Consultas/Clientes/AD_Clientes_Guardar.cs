using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Consultas.ClientesDatosPersonaFisica;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_Guardar
    {
        private string CadenaConexion;
        public AD_Clientes_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<int> Guardar_Persona_Moral(mdlClientes mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
   
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    rfc = mdl.rfc,
                    razon_social = mdl.razon_social,
                    tipo_persona = mdl.tipo_persona,
                    medio_contacto = mdl.medio_contacto,
                    tiempo_agricultor = mdl.tiempo_agricultor,
                    agrupacion = mdl.agrupacion,
                    regimen_fiscal = mdl.regimen_fiscal,
                    tipo_venta = mdl.tipo_venta,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                mdl.idcliente= await factory.SQL.QueryFirstAsync<int>("Credito.sp_clientes_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return mdl.idcliente;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<int> Guardar_Persona_Fisica(mdlClientes_Datos_Persona_Fisica mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    rfc = mdl.rfc,
                    razon_social = mdl.razon_social,
                    tipo_persona = mdl.tipo_persona,
                    medio_contacto = mdl.medio_contacto,
                    tiempo_agricultor = mdl.tiempo_agricultor,
                    agrupacion = mdl.agrupacion,
                    regimen_fiscal = mdl.regimen_fiscal,
                    tipo_venta = mdl.tipo_venta,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                mdl.idcliente = await factory.SQL.QueryFirstAsync<int>("Credito.sp_clientes_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                if (mdl.idcliente > 0)
                {
                    factory.SQL.Close();
                    await new AD_ClientesDatosPersonaFisica_Guardar(CadenaConexion).Guardar(mdl);
                }
                else
                {
                    factory.SQL.Close();
                }
                return mdl.idcliente;
            }
            catch (Exception ex)
            {
                //factory.transaccion.Rollback();
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
