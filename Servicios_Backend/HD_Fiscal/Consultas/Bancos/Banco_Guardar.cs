using Dapper;
using HD_AccesoDatos;
using HD_Fiscal.Modelos;
using System;
using System.Threading.Tasks;

namespace HD_Fiscal.Consultas.Bancos
{
    public class Banco_Guardar : FactoryConectionBase
    {
        public Banco_Guardar(string cadenaconexion)
            : base(cadenaconexion)
        {

        }
        public async Task<bool> Guardar(mdlBancos mdl)
        {
            try
            {
                var parametros = new
                {
                    idbanco = mdl.idbanco,
                    nombre = mdl.nombre,
                    cuenta = mdl.cuenta,
                    sucursal = mdl.sucursal,
                    moneda = mdl.moneda,
                    clave_interbancaria = mdl.clave_interbancaria,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("sp_Bancos_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                Mensaje = "Datos registrados con exito";
                return true;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return false;
            }
        }
    }
}
