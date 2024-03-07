using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Tasa_de_intereses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.Tasa_de_intereses
{
    public class FAD_TipoTasasValores_Guardar
    {
        private string CadenaConexion;
        public FAD_TipoTasasValores_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(Fmdl_TipoTasasValores mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idtipo_tasa_valor = mdl.idtipo_tasa_valor,
                    idtipo_tasa = mdl.idtipo_tasa,
                    plazo = mdl.plazo,
                    porcentaje = mdl.porcentaje,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.sp_Tipo_Tasas_Valores_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
