using HD.AccesoDatos;
using HD_CentroMonitoreo.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Consultas
{
    public class AD_Catalogo_Alertas
    {
        private readonly string CadenaConexion;

        public AD_Catalogo_Alertas(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        public async Task<IEnumerable<mdl_Catalogo_Alerta_Maquinaria>> ObtenerCatalogo()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Catalogo_Alerta_Maquinaria> result =
                    await factory.SQL.QueryAsync<mdl_Catalogo_Alerta_Maquinaria>(
                        "HumayaDigital_Eventos.csc.sp_Get_Catalogo_Alertas_Maquinaria",
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Alerta_DDL>> ListadoAlertasDDL()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Alerta_DDL> result =
                    await factory.SQL.QueryAsync<mdl_Alerta_DDL>(
                        "HumayaDigital_Eventos.csc.sp_Listado_Alertas_DDL",
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<int> GuardarAlerta(mdl_Guardar_Catalogo_Alerta mdl)
        {
            try
            {
                var parametros = new
                {
                    id_alerta = mdl.id_alerta,
                    descripcion_alerta = mdl.descripcion_alerta,
                    descripcion_cliente = mdl.descripcion_cliente,
                    usuario = mdl.usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<int>(
                    "HumayaDigital_Eventos.csc.sp_Guarda_Catalogo_Alerta",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> EliminarAlerta(int id_alerta)
        {
            try
            {
                var parametros = new { id_alerta };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync(
                    "HumayaDigital_Eventos.csc.so_Elimina_Catalogo_Alerta",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }
    }
}
