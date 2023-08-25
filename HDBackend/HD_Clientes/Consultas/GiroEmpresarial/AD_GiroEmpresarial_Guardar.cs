using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.GiroEmpresarial
{
    public class AD_GiroEmpresarial_Guardar
    {
        private string CadenaConexion;
        public AD_GiroEmpresarial_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlGiro_Empresarial mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idgiro_empresarial = mdl.idgiro_empresarial,
                    descripcion = mdl.descripcion,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_giro_empresarial_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
