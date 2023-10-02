using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.ClientesCultivo
{
    public class AD_ClientesCultivo_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesCultivo_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientes_Cultivo_Listado>> Guardar(mdlClientes_Cultivo mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    registro = mdl.registro,
                    idcultivo= mdl.idcultivo,
                    terreno = mdl.terreno,
                    hectareas = mdl.hectareas,
                    seguro_cosecha = mdl.seguro_cosecha,
                    ciclo = mdl.ciclo,
                    tipo_riego = mdl.tipo_riego,
                    temporal = mdl.temporal,
                    rendimiento = mdl.rendimiento,
                    precio = mdl.precio,
                    mescosecha = mdl.mescosecha,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                IEnumerable<mdlClientes_Cultivo_Listado> result = await factory.SQL.QueryAsync<mdlClientes_Cultivo_Listado>("Credito.sp_Clientes_Cultivo_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
