using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.TazaInteres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.tasa_interes
{
    public class ADTasa_Interes
    {
        private string CadenaConexion;
        public ADTasa_Interes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdltasadropdownlist>> Buscartasas()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdltasadropdownlist> result = await factory.SQL.QueryAsync<mdltasadropdownlist>("Credito.sp_Tipotasa_dropdownlist", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdltasadropdownlist>> Buscar_Tasas_valores(int idtasa)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idtasa
                };
                IEnumerable<mdltasadropdownlist> result = await factory.SQL.QueryAsync<mdltasadropdownlist>("Credito.sp_tipotasa_valores_dropdownlist", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
