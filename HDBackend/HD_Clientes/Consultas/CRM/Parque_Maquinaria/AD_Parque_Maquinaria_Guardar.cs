using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Parque_Maquinaria;
using System.Data.SqlClient;

namespace HD.Clientes.Consultas.CRM.Parque_Maquinaria
{
    public class AD_Parque_Maquinaria_Guardar
    {
        private string CadenaConexion;
        public AD_Parque_Maquinaria_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Da de alta o edita una maquina del parque de maquinaria del cliente.
        /// El SP resuelve el alta cuando idrelacion viene en -99 y devuelve el
        /// idrelacion final, para que el front refresque el renglon sin recargar
        /// todo el listado.
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000)
        /// son validaciones del SP con mensaje para el usuario final y se devuelven
        /// tal cual como BadRequest.
        /// </summary>
        public async Task<int> Guardar(mdl_Guardar_Parque_MaquinariaCRM mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    idrelacion = mdl.idrelacion,
                    idcliente = mdl.idcliente,
                    categoria = mdl.categoria,
                    tipo = mdl.tipo,
                    marca = mdl.marca,
                    modelo = mdl.modelo,
                    serie = mdl.serie,
                    anio = mdl.anio,
                    comentarios = mdl.comentarios,
                    usuario = mdl.usuario
                };

                mdl.idrelacion = await factory.SQL.QueryFirstAsync<int>("CRM.sp_Parque_Maquinaria_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return mdl.idrelacion;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
