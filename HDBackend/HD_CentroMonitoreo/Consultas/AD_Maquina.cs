using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_CentroMonitoreo.Modelos;

namespace HD_CentroMonitoreo.Consultas.Maquina
{
    public class AD_Maquina
    {
        private string CadenaConexion;

        public AD_Maquina(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<List<mdl_Maquina>> PorOrganizacion(string jd_org_id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    jd_org_id = jd_org_id
                };

                var result = await factory.SQL.QueryAsync<mdl_Maquina>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Maquina_PorOrganizacion", 
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_MaquinaDetalle> Detalle(string jd_machine_id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    jd_machine_id = jd_machine_id
                };

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_MaquinaDetalle>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Maquina_Detalle", 
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<List<mdl_EstadoMaquina>> EstadoPorMaquina(string maquina_id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    maquina_id = maquina_id
                };

                var result = await factory.SQL.QueryAsync<mdl_EstadoMaquina>(
                    "HumayaDigital_Eventos.csc.SP_Cat_EstadoMaquina_PorMaquina",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<List<mdl_Alerta>> AlertasPorMaquina(string maquina_id)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    maquina_id = maquina_id
                };

                var result = await factory.SQL.QueryAsync<mdl_Alerta>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Alerta_PorMaquina",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<List<mdl_Ubicacion>> RecorridoPorFecha(string maquina_id, DateTime fecha)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    maquina_id = maquina_id,
                    fecha = fecha.Date
                };

                var result = await factory.SQL.QueryAsync<mdl_Ubicacion>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Ubicacion_PorMaquinaFecha",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }





        public async Task<List<mdl_Ubicacion>> RecorridoPorRango(string maquina_id, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    maquina_id = maquina_id,
                    fecha_inicio = fechaInicio?.Date,
                    fecha_fin = fechaFin?.Date
                };

                var result = await factory.SQL.QueryAsync<mdl_Ubicacion>(
                    "HumayaDigital_Eventos.csc.SP_Cat_Ubicacion_PorMaquinaRango",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<List<mdl_HorasMotor>> HorasMotorPorRango(string maquina_id, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    maquina_id = maquina_id,
                    fecha_inicio = fechaInicio?.Date,
                    fecha_fin = fechaFin?.Date
                };

                var result = await factory.SQL.QueryAsync<mdl_HorasMotor>(
                    "HumayaDigital_Eventos.csc.SP_Cat_HorasMotor_PorMaquinaRango",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );

                factory.SQL.Close();
                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

    }
}   