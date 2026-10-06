using Dapper;
using HD.AccesoDatos;
using HD_CentroMonitoreo.Modelos;
using HD_CentroMonitoreo.Modelos.Dashboard;
using System.Data.SqlClient;

namespace HD_CentroMonitoreo.Consultas.Dashboard
{
    public class AD_Dashboard_Obtener
    {
        private string CadenaConexion;
        public AD_Dashboard_Obtener(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Arma el dashboard de Soluciones Integrales con UN solo SP de 7 selects, en este orden:
        ///   1) KPIs                  (1 fila)  -> mdl_Dashboard_Kpis_Fila
        ///   2) Mensajeria            (1 fila)  -> mdl_Dashboard_Mensajeria_Fila
        ///   3) Mensajeria por tipo   (n filas) -> mdl_Dashboard_MensajeriaTipo
        ///   4) Resumen organizaciones(1 fila)  -> mdl_Dashboard_Organizaciones_Fila
        ///   5) Sucursales            (n filas) -> mdl_Dashboard_Sucursal
        ///   6) Atencion requerida    (n filas) -> mdl_Dashboard_Atencion_Fila
        ///   7) Organizaciones con mas equipos (n filas) -> mdl_Organizacion
        /// El SP recibe el rango consultado y el periodo anterior (calculado aqui), las listas
        /// de sucursal / adr y el usuario de la sesion: el SP debe intersectar la sucursal pedida
        /// con las sucursales que el usuario tiene permitidas (nunca confiar solo en los ids del front).
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) se devuelven
        /// como BadRequest; cualquier otro error responde un mensaje generico y el detalle se
        /// registra solo en el servidor.
        /// </summary>
        public async Task<mdl_Dashboard_View> Obtener(mdl_Dashboard_Filtro filtro, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var anterior = Dashboard_PeriodoAnterior.Calcular(filtro);

                var parametros = new
                {
                    ejercicio_inicio = filtro.ejercicioinicio,
                    periodo_inicio = filtro.periodoinicio,
                    ejercicio_fin = filtro.ejerciciofin,
                    periodo_fin = filtro.periodofin,
                    ejercicio_inicio_anterior = anterior.ejercicio_inicio,
                    periodo_inicio_anterior = anterior.periodo_inicio,
                    ejercicio_fin_anterior = anterior.ejercicio_fin,
                    periodo_fin_anterior = anterior.periodo_fin,
                    sucursal = filtro.SucursalNormalizada(),
                    adr = filtro.AdrNormalizada(),
                    usuario = usuario
                };

                mdl_Dashboard_Kpis_Fila kpis;
                mdl_Dashboard_Mensajeria_Fila mensajeria;
                List<mdl_Dashboard_MensajeriaTipo> tipos;
                mdl_Dashboard_Organizaciones_Fila resumen;
                mdl_Dashboard_Organizaciones_Tecnologias tecnologias;
                List<mdl_Dashboard_Sucursal> sucursales;
                List<mdl_Dashboard_Atencion_Fila> atencion;
                List<mdl_Organizacion> top;

                using (var multi = await factory.SQL.QueryMultipleAsync("HumayaDigital_Eventos.csc.sp_CentroMonitoreo_Dashboard", parametros, commandType: System.Data.CommandType.StoredProcedure))
                {
                    kpis = (await multi.ReadAsync<mdl_Dashboard_Kpis_Fila>()).FirstOrDefault() ?? new mdl_Dashboard_Kpis_Fila();
                    mensajeria = (await multi.ReadAsync<mdl_Dashboard_Mensajeria_Fila>()).FirstOrDefault() ?? new mdl_Dashboard_Mensajeria_Fila();
                    tipos = (await multi.ReadAsync<mdl_Dashboard_MensajeriaTipo>()).ToList();
                    resumen = (await multi.ReadAsync<mdl_Dashboard_Organizaciones_Fila>()).FirstOrDefault() ?? new mdl_Dashboard_Organizaciones_Fila();
                    tecnologias = (await multi.ReadAsync<mdl_Dashboard_Organizaciones_Tecnologias>()).FirstOrDefault() ?? new mdl_Dashboard_Organizaciones_Tecnologias();
                    sucursales = (await multi.ReadAsync<mdl_Dashboard_Sucursal>()).ToList();
                    atencion = (await multi.ReadAsync<mdl_Dashboard_Atencion_Fila>()).ToList();
                    top = (await multi.ReadAsync<mdl_Organizacion>()).ToList();
                }

                factory.SQL.Close();

                var vista = new mdl_Dashboard_View
                {
                    kpis = new mdl_Dashboard_Kpis
                    {
                        organizaciones = new mdl_Dashboard_Valor { actual = kpis.organizaciones, anterior = kpis.organizaciones_anterior },
                        equipos_registrados = new mdl_Dashboard_Valor { actual = kpis.equipos_registrados, anterior = kpis.equipos_registrados_anterior },
                        equipos_reportando = new mdl_Dashboard_Reportando
                        {
                            actual = kpis.equipos_reportando,
                            anterior = kpis.equipos_reportando_anterior,
                            total_equipos = kpis.equipos_registrados,
                            total_equipos_anterior = kpis.equipos_registrados_anterior
                        },
                        mensajes_leidos = new mdl_Dashboard_Valor { actual = kpis.mensajes_leidos, anterior = kpis.mensajes_leidos_anterior }
                    },
                    mensajeria = new mdl_Dashboard_Mensajeria
                    {
                        generados = mensajeria.generados,
                        enviados = mensajeria.enviados,
                        entregados = mensajeria.entregados,
                        leidos = mensajeria.leidos,
                        pendientes = mensajeria.pendientes,
                        con_error = mensajeria.con_error,
                        error_principal = new mdl_Dashboard_ErrorPrincipal
                        {
                            causa = mensajeria.error_causa ?? string.Empty,
                            total = mensajeria.error_total
                        },
                        por_tipo = tipos
                    },
                    organizaciones = new mdl_Dashboard_Organizaciones
                    {
                        total = resumen.total,
                        con_equipos_reportando = resumen.con_equipos_reportando,
                        sin_reporte = resumen.sin_reporte,
                        total_equipos = resumen.total_equipos,
                        tractores = resumen.tractores,
                        datos_reportados = new mdl_Dashboard_DatosReportados
                        {
                            horometro = tecnologias.horometro,
                            ubicacion = tecnologias.ubicacion,
                            combustible = tecnologias.combustible,
                            alertas = tecnologias.alertas
                        },
                        datos_reportados_organizaciones = new mdl_Dashboard_DatosReportados
                        {
                            horometro = tecnologias.organizaciones_horometro,
                            ubicacion = tecnologias.organizaciones_ubicacion,
                            combustible = tecnologias.organizaciones_combustible,
                            alertas = tecnologias.organizaciones_alertas
                        },
                        equipos_completos=tecnologias.equipos_completos,
                        sucursales = sucursales
                    },
                    atencion_requerida = ArmarAtencion(atencion),
                    organizaciones_mas_equipos = top.OrderByDescending(o => o.total_equipos).ToList(),
                    fecha_actualizacion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                return vista;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                // El detalle tecnico se queda en el servidor; al cliente solo un mensaje generico.
                Console.WriteLine($"[CentroMonitoreo/Dashboard] {ex}");
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = "No fue posible obtener la información del dashboard. Intenta nuevamente en unos minutos." });
            }
        }

        // Prioridades validas: critica > alta > media. Cualquier otro valor se trata como media.
        // El orden dentro de una misma prioridad respeta el que regresa el SP.
        private static List<mdl_Dashboard_Atencion> ArmarAtencion(List<mdl_Dashboard_Atencion_Fila> filas)
        {
            int Rango(string prioridad) => prioridad switch
            {
                "critica" => 0,
                "alta" => 1,
                _ => 2
            };

            string Normalizar(string? prioridad)
            {
                string p = (prioridad ?? string.Empty).Trim().ToLowerInvariant();
                return p == "crítica" ? "critica" : (p == "critica" || p == "alta" || p == "media") ? p : "media";
            }

            return filas
                .Select(f => new mdl_Dashboard_Atencion
                {
                    prioridad = Normalizar(f.prioridad),
                    titulo = f.titulo,
                    descripcion = f.descripcion,
                    jd_org_id = string.IsNullOrWhiteSpace(f.jd_org_id) ? null : f.jd_org_id,
                    fecha_evento=f.fecha_evento
                })
                .OrderBy(a => Rango(a.prioridad))
                .Select((a, i) => { a.id = i + 1; return a; })
                .ToList();
        }
    }
}
