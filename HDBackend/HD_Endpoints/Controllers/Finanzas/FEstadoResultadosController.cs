using HD.Security;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.AccesoDatos.Actions;
using HD_Finanzas.Modelos.Actions;
using HD_Finanzas.Modelos.Estado_Resultados;
using HD_Reporteria;
using HD_Reporteria.Finanzas;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEstadoResultadosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEstadoResultadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FiltroEscenariosER()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_EstadoResultados datos = new FAD_EstadoResultados(CadenaConexion);
            var result = await datos.Get_Filtro_Escenarios();
            return Ok(result);
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadosByDireccionRolado(Fmdl_EstadoResultadosRolado prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string usuario = Sesion.usuario();
            FAD_EstadoResultados estadoresultados = new FAD_EstadoResultados(CadenaConexion);
            var result = await estadoresultados.GetEstadoResultadosByDireccionRolado(prm, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadosEbitda(Fmdl_EstadoResultadosRolado prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string usuario = Sesion.usuario();
            FAD_EstadoResultados estadoresultados = new FAD_EstadoResultados(CadenaConexion);
            var result = await estadoresultados.GetEstadoResultadosEbitda(prm, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadoGrafica(Fmdl_Estado_Resultados_Grafica_Filtro prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_EstadiResultados_Grafica estadoresultados = new FAD_EstadiResultados_Grafica(CadenaConexion);
            var result = await estadoresultados.EstadoResultadosGrafica(prm);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(Fmdl_EstadoResultados_PDF prm)
        {
            try
            {
                RPT_Result documento = RPT_Finanzas_EstadoResultados.Generar(prm);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(Fmdl_EstadoResultados_PDF vm)
        {
            var docresult = await XLS_EstadoResultados.EstadoResultados(vm);
            return Ok(docresult);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelTodos(Fmdl_EstadoResultadosRolado prm)
        {
            var ejercicios = new List<int> { 2020, 2021, 2022, 2023, 2024, 2025, 2026 };
            var meses = Enumerable.Range(1, 12);

            var sucursales = new List<string> { "1", "11", "21", "31", "41", "51", "61", "2", "12", "22", "32", "52" };
            var sucursalesNombre = new List<string> { "NAVOLATO", "CAIMANERO", "EL DORADO", "COSTA RICA", "LA CRUZ", "EL ROSARIO", "VILLA UNION", "TEPIC", "SAN JOSE", "SANTIAGO I", "TECUALA", "SAN VICENTE" };

            var regionPorSucursal = new Dictionary<string, Fmdl_EstadoResultados_Region>
    {
        { "1", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "11", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "21", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "31", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "41", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "51", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },
        { "61", new Fmdl_EstadoResultados_Region { idadr = 1, adr = "Sinaloa" } },

        { "2", new Fmdl_EstadoResultados_Region { idadr = 2, adr = "Nayarit" } },
        { "12", new Fmdl_EstadoResultados_Region { idadr = 2, adr = "Nayarit" } },
        { "22", new Fmdl_EstadoResultados_Region { idadr = 2, adr = "Nayarit" } },
        { "32", new Fmdl_EstadoResultados_Region { idadr = 2, adr = "Nayarit" } },
        { "52", new Fmdl_EstadoResultados_Region { idadr = 2, adr = "Nayarit" } }
    };

            string cadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string usuario = Sesion.usuario();

            var estadoresultados = new FAD_EstadoResultados(cadenaConexion);

            string rutaBase = @"C:\Users\Desarrollador TI\Documents\EstadoResultados";

            foreach (var ejercicio in ejercicios)
            {
                // 📁 Crear carpeta del ejercicio
                string rutaEjercicio = Path.Combine(rutaBase, ejercicio.ToString());
                Directory.CreateDirectory(rutaEjercicio);

                foreach (var mes in meses)
                {
                    var fechaInicio = new System.DateTime(ejercicio, mes, 1);
                    var fechaFin = new System.DateTime(ejercicio, mes, System.DateTime.DaysInMonth(ejercicio, mes));

                    string nombreMes = fechaInicio
                        .ToString("MMMM", new System.Globalization.CultureInfo("es-MX"))
                        .ToUpper();

                    for (int i = 0; i < sucursales.Count; i++)
                    {
                        var sucursal = sucursales[i];
                        var nombreSucursal = sucursalesNombre[i];

                        prm.fechainicio = fechaInicio.ToString("yyyy-MM-dd");
                        prm.fechafin = fechaFin.ToString("yyyy-MM-dd");
                        prm.sucursal = sucursal;

                        var result = await estadoresultados
                            .GetEstadoResultadosByDireccionRolado(prm, usuario);

                        var dataAgrupada = result
                            .GroupBy(x => x.departamento ?? "SIN DEPARTAMENTO")
                            .Select(g => new Fmdl_EstadoResultados_Data
                            {
                                depto = g.Key,
                                data = g.ToList()
                            })
                            .ToList();

                        var vm = new Fmdl_EstadoResultados_PDF
                        {
                            periodoactual = $"{nombreMes} DE {ejercicio}",
                            periodoanterior = $"{nombreMes} DE {ejercicio - 1}",
                            region = new List<Fmdl_EstadoResultados_Region>
                    {
                        regionPorSucursal[sucursal]
                    },
                            subtitulo = $"{nombreMes} DE {ejercicio}",
                            sucursal = new List<Fmdl_EstadoResultados_Sucursal>
                    {
                        new Fmdl_EstadoResultados_Sucursal
                        {
                            idsucursal = int.Parse(sucursal),
                            sucursal = nombreSucursal
                        }
                    },
                            data = dataAgrupada
                        };

                        // 🧹 Limpiar nombres
                        string nombreMesLimpio = nombreMes.Replace(" ", "_");
                        string nombreSucursalLimpio = nombreSucursal.Replace(" ", "_");

                        string nombreArchivo = $"{nombreMesLimpio}_{nombreSucursalLimpio}.xlsx";

                        // eliminar caracteres inválidos
                        foreach (var c in Path.GetInvalidFileNameChars())
                        {
                            nombreArchivo = nombreArchivo.Replace(c, '_');
                        }

                        string rutaArchivo = Path.Combine(rutaEjercicio, nombreArchivo);

                        // 💾 Generar y guardar archivo
                        await XLS_EstadoResultadosTodos.EstadoResultados(vm, rutaArchivo);
                    }
                }
            }

            return Ok("Proceso completado");
        }
    }
}
