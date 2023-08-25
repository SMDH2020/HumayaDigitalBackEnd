using Dapper;
using HD.AccesoDatos;
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
        public async Task<bool> Guardar(mdlClientes mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
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
                await factory.SQL.QueryAsync("Credito.sp_clientes_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
