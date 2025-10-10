using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Editar_Modelo
    {
        private string CadenaConexion;
        public AD_Editar_Modelo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> EditarModelo(mdl_Editar_Modelo mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idmodelo = mdl.idmodelo,
                    idlinea = mdl.idlinea,
                    modelo = mdl.modelo,
                    descripcion_mdl = mdl.descripcion_mdl,
                    costo_refacciones = mdl.costo_refacciones,
                    costo_servicios = mdl.costo_servicios,
                    precio_lista = mdl.precio_lista,
                    moneda = mdl.moneda,
                    usuario = mdl.usuario,
                    caracteristicas = mdl.caracteristicas,
                    imagenes = mdl.imagenes
                };
                await factory.SQL.QueryAsync("Ventas.Editar_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Modelos>> CambiarEstado(int idmodelo)
        {
            try
            {
                var parametros = new
                {
                    idmodelo = idmodelo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Modelos> result = await factory.SQL.QueryAsync<mdl_Listado_Modelos>("Ventas.sp_Cambiar_Estado_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
