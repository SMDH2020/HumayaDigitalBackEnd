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
    public class AD_Agregar_Modelo
    {
        private string CadenaConexion;
        public AD_Agregar_Modelo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> AgregarModelo(mdl_Agregar_Modelo mdl)
        {
           
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idlinea = mdl.idlinea,
                    modelo = mdl.modelo,
                    descripcion_mdl = mdl.descripcion_mdl,
                    costo_refacciones = mdl.costo_refacciones,
                    costo_servicios = mdl.costo_servicio,
                    precio_lista = mdl.precio_lista,
                    moneda = mdl.moneda,
                    usuario = mdl.usuario,
                    categoria = mdl.categoria,
                    caracteristicas = mdl.caracteristicas,
                    imagenes = mdl.imagenes
                };
                await factory.SQL.QueryAsync("Ventas.Guardar_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
