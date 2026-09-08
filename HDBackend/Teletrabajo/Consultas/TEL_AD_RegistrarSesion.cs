using Dapper;
using HD.AccesoDatos;
using Teletrabajo.Modelos;

namespace Teletrabajo.Consultas
{
    public class TEL_AD_RegistrarSesion
    {
        private string CadenaConexion;
        public TEL_AD_RegistrarSesion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<string>> PrimerRegistro(TEL_mdl_InfoSesion mdl)
        {
            try
            {
                var parametros = new
                {
                    idusuario = mdl.usuario,
                    serie = mdl.serie,
                    modelo = mdl.modelo,
                    marca = mdl.marca,
                    mac = mdl.mac
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<string> result = await factory.SQL.QueryAsync<string>("HumayaDigital_Usuarios.dbo.sp_Usuarios_Teletrabajo_Login", parametros, commandType: System.Data.CommandType.StoredProcedure);
                //TEL_mdl_result modelo = new TEL_mdl_result();
                //TEL_mdl_Lecturas lec = result.Read< TEL_mdl_Lecturas >().FirstOrDefault();
                //modelo.registros = result.Read<string>().ToList();
                //modelo.empleado = lec.empleado;
                //modelo.sucursal = lec.sucursal;
                //modelo.puesto = lec.puesto;
                //modelo.foto = Convert.ToBase64String(lec.foto);

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<TEL_mdl_result> Acceso(TEL_mdl_InfoSesion mdl)
        {
            try
            {
                var parametros = new
                {
                    idusuario = mdl.usuario,
                    serie = mdl.serie,
                    modelo = mdl.modelo,
                    marca = mdl.marca,
                    mac = mdl.mac
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("HumayaDigital_Usuarios.dbo.sp_Usuarios_Teletrabajo_Lecturas", parametros, commandType: System.Data.CommandType.StoredProcedure);

                TEL_mdl_result modelo = new TEL_mdl_result();
                TEL_mdl_Lecturas lec = result.Read<TEL_mdl_Lecturas>().FirstOrDefault();
                modelo.registros = result.Read<string>().ToList();
                modelo.empleado = lec.empleado;
                modelo.sucursal = lec.sucursal;
                modelo.puesto = lec.puesto;
                modelo.foto = Convert.ToBase64String(lec.foto);

                factory.SQL.Close();
                return modelo;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
