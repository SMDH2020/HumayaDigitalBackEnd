using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.ProyeccionesVentas;

namespace HD_Finanzas.AccesoDatos
{
    public class AD_ProyeccionesVentas
    {
        private string CadenaConexion;
        public AD_ProyeccionesVentas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<List<mdl_Proyecciones_Venta>> ObtenerProyeccion(mdl_Filtro_Proyecciones_Ventas vm, string Usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    Ejercicio = vm.ejercicio,
                    Ejercicioant = vm.ejercicioant,
                    escenario = vm.escenario,
                    comparar = vm.comparar,
                    Periodos = vm.periodo,
                    Departamentos = vm.departamento,
                    Sucursales = vm.sucursal,
                    ADR = vm.adr,
                    Usuario = Usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.SP_Revision_ProyeccionVentas_HumayaDigital_Escenarios", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var Proy = result.Read<mdl_Estado_Resultados>().ToList();
                var ER = result.Read<mdl_Estado_Resultados>().ToList();
                var ProyGastos = result.Read<mdl_Estado_Resultados>().ToList();
                var ERGastos = result.Read<mdl_Estado_Resultados>().ToList();
                var ProyFinancieros = result.Read<mdl_Estado_Resultados>().ToList();
                var ERFinancieros = result.Read<mdl_Estado_Resultados>().ToList();

                List<mdl_Proyecciones_Venta> view = new List<mdl_Proyecciones_Venta>();
                int index = 0;
                foreach (mdl_Estado_Resultados objProy in ER)
                {
                    mdl_Proyecciones_Venta obj = new mdl_Proyecciones_Venta();
                    obj.index = index;
                    obj.concepto = objProy.concepto;
                    obj.departamento = objProy.departamento;
                    obj.proyimporte = Math.Round(objProy.importe);
                    obj.orden = objProy.orden;
                    obj.proypor = Math.Round(objProy.por, 2);

                    var objER = Proy.Where(x => x.departamento.Equals(objProy.departamento) && x.concepto.Equals(objProy.concepto)).FirstOrDefault();

                    obj.importe = objER is null ? 0 : Math.Round(objER.importe, 2);
                    obj.por = objER is null ? 0 : Math.Round(objER.por, 2);

                    obj.indicador = ObtenerIndicador(true, obj.concepto, obj.importe, obj.proyimporte, obj.por, obj.proypor);
                    obj.diffimporte = Math.Round(obj.importe - obj.proyimporte, 2);
                    obj.diffpor = obj.importe == 0 || obj.proyimporte == 0 ? 0
                        : Math.Round((obj.importe - obj.proyimporte) / obj.proyimporte * 100, 2);
                    view.Add(obj);
                    index++;
                }

                double ProyVentasNetas = view.Where(x => x.orden == 3).Sum(x => x.importe);
                double RealVentasNetas = view.Where(x => x.orden == 3).Sum(x => x.proyimporte);
                double proyCostoVenta = view.Where(x => x.orden == 4).Sum(x => x.importe);
                double realCostoVenta = view.Where(x => x.orden == 4).Sum(x => x.proyimporte);

                view.Add(new mdl_Proyecciones_Venta
                {
                    index = index,
                    departamento = "VENTAS NETAS",
                    concepto = "Total Ventas Netas",
                    orden = 10,
                    importe = Math.Round(ProyVentasNetas, 2),
                    por = 100,
                    proyimporte = Math.Round(RealVentasNetas, 2),
                    indicador = ObtenerIndicador(false, "Total Ventas Netas", ProyVentasNetas, RealVentasNetas, 100, 100),
                    proypor = 100,
                    diffimporte = Math.Round(ProyVentasNetas - RealVentasNetas, 2),
                    diffpor = ProyVentasNetas == 0 || RealVentasNetas == 0 ? 0
                        : Math.Round((ProyVentasNetas - RealVentasNetas) / RealVentasNetas * 100, 2)
                });
                index += 1;
                view.Add(new mdl_Proyecciones_Venta
                {
                    index = index,
                    departamento = "VENTAS NETAS",
                    concepto = "Costos de Venta",
                    orden = 10,
                    importe = Math.Round(proyCostoVenta, 2),
                    por = ProyVentasNetas == 0 || proyCostoVenta == 0 ? 0
                    : Math.Round(proyCostoVenta / ProyVentasNetas * 100, 2),
                    proyimporte = Math.Round(realCostoVenta, 2),
                    proypor = RealVentasNetas == 0 || realCostoVenta == 0 ? 0
                    : Math.Round(realCostoVenta / RealVentasNetas * 100, 2),
                    indicador = ObtenerIndicador(false, "Costos de Venta", proyCostoVenta, realCostoVenta, ProyVentasNetas == 0 || proyCostoVenta == 0 ? 0
                    : Math.Round(proyCostoVenta / ProyVentasNetas * 100, 2), RealVentasNetas == 0 || realCostoVenta == 0 ? 0
                    : Math.Round(realCostoVenta / RealVentasNetas * 100, 2)),
                    diffimporte = Math.Round(proyCostoVenta - realCostoVenta, 2),
                    diffpor = proyCostoVenta == 0 || realCostoVenta == 0 ? 0
                        : Math.Round((proyCostoVenta - realCostoVenta) / realCostoVenta * 100, 2)
                });

                index += 1;

                double proyutilidad = ProyVentasNetas - proyCostoVenta;
                double realutilidad = RealVentasNetas - realCostoVenta;
                view.Add(new mdl_Proyecciones_Venta
                {
                    index = index,
                    departamento = "VENTAS NETAS",
                    concepto = "Utilidad Bruta",
                    orden = 10,
                    importe = Math.Round(proyutilidad, 2),
                    por = ProyVentasNetas == 0 || proyutilidad == 0 ? 0
                    : Math.Round(proyutilidad / ProyVentasNetas * 100, 2),
                    proyimporte = Math.Round(realutilidad, 2),
                    proypor = RealVentasNetas == 0 || realutilidad == 0 ? 0
                    : Math.Round(realutilidad / RealVentasNetas * 100, 2),
                    indicador = ObtenerIndicador(false, "Utilidad Bruta", proyutilidad, realutilidad, ProyVentasNetas == 0 || proyutilidad == 0 ? 0
                    : Math.Round(proyutilidad / ProyVentasNetas * 100, 2), RealVentasNetas == 0 || realutilidad == 0 ? 0
                    : Math.Round(realutilidad / RealVentasNetas * 100, 2)),
                    diffimporte = Math.Round(proyutilidad - realutilidad, 2),
                    diffpor = proyutilidad == 0 || realutilidad == 0 ? 0
                        : Math.Round((proyutilidad - realutilidad) / realutilidad * 100, 2)
                });


                index += 1;
                double proyGastos = view.Where(x => x.orden == 6).Sum(x => x.importe);
                double realGastos = view.Where(x => x.orden == 6).Sum(x => x.proyimporte);

                view.Add(new mdl_Proyecciones_Venta
                {
                    index = index,
                    departamento = "VENTAS NETAS",
                    concepto = "Gastos Departamentales",
                    orden = 10,
                    importe = Math.Round(proyGastos, 2),
                    por = ProyVentasNetas == 0 || proyGastos == 0 ? 0
                    : Math.Round(proyGastos / ProyVentasNetas * 100, 2),
                    proyimporte = Math.Round(realGastos, 2),
                    proypor = RealVentasNetas == 0 || realGastos == 0 ? 0
                    : Math.Round(realGastos / RealVentasNetas * 100, 2),
                    indicador = ObtenerIndicador(false, "Gastos de Departamento", proyGastos, realGastos, ProyVentasNetas == 0 || proyGastos == 0 ? 0
                    : Math.Round(proyGastos / ProyVentasNetas * 100, 2), RealVentasNetas == 0 || realGastos == 0 ? 0
                    : Math.Round(realGastos / RealVentasNetas * 100, 2)),
                    diffimporte = Math.Round(proyGastos - realGastos, 2),
                    diffpor = proyGastos == 0 || realGastos == 0 ? 0
                        : Math.Round((proyGastos - realGastos) / realGastos * 100, 2)
                });


                index += 1;
                foreach (mdl_Estado_Resultados gst in ERGastos)
                {
                    mdl_Proyecciones_Venta vgst = new mdl_Proyecciones_Venta();
                    vgst.index = index;
                    vgst.departamento = "VENTAS NETAS";
                    vgst.concepto = $"Gastos de {gst.concepto.ToLower()}";

                    var pgst = ProyGastos.Where(x => x.departamento == gst.departamento).FirstOrDefault();
                    vgst.importe = pgst is null ? 0 : pgst.importe;
                    vgst.por = ProyVentasNetas == 0 || vgst.importe == 0 ? 0
                           : Math.Round(vgst.importe / ProyVentasNetas * 100, 2);

                    vgst.proyimporte = gst.importe;
                    vgst.proypor = RealVentasNetas == 0 || vgst.proyimporte == 0 ? 0
                        : Math.Round(vgst.proyimporte / RealVentasNetas * 100, 2);

                    vgst.indicador = ObtenerIndicador(false, "Gastos de Departamento", vgst.importe, vgst.proyimporte, vgst.por, vgst.proypor);

                    vgst.diffimporte = vgst.importe - vgst.proyimporte;
                    vgst.diffpor = vgst.diffimporte == 0 || vgst.proyimporte == 0 ? 0
                        : Math.Round(vgst.diffimporte / vgst.proyimporte * 100, 2);

                    view.Add(vgst);
                }



                index += 1;
                double proyOperacion = proyutilidad - proyGastos - ProyGastos.Sum(x => x.importe);
                double realOperacion = realutilidad - realGastos - ERGastos.Sum(x => x.importe);

                view.Add(new mdl_Proyecciones_Venta
                {
                    index = index,
                    departamento = "VENTAS NETAS",
                    concepto = "Utilidad de Operación",
                    orden = 10,
                    importe = Math.Round(proyOperacion, 2),
                    por = ProyVentasNetas == 0 || proyOperacion == 0 ? 0
                    : Math.Round(proyOperacion / ProyVentasNetas * 100, 2),
                    proyimporte = Math.Round(realOperacion, 2),
                    proypor = RealVentasNetas == 0 || realOperacion == 0 ? 0
                    : Math.Round(realOperacion / RealVentasNetas * 100, 2),
                    indicador = ObtenerIndicador(false, "Utilidad de Operación", proyOperacion, realOperacion, ProyVentasNetas == 0 || proyOperacion == 0 ? 0
                    : Math.Round(proyOperacion / ProyVentasNetas * 100, 2), RealVentasNetas == 0 || realOperacion == 0 ? 0
                    : Math.Round(realOperacion / RealVentasNetas * 100, 2)),
                    diffimporte = Math.Round(proyOperacion - realOperacion, 2),
                    diffpor = proyOperacion == 0 || realOperacion == 0 ? 0
                        : Math.Round((proyOperacion - realOperacion) / realOperacion * 100, 2)
                });


                foreach (var fin in ProyFinancieros)
                {
                    var importefinanciero = ERFinancieros.Where(x => x.concepto == fin.concepto && x.departamento == fin.departamento).FirstOrDefault();
                    index += 1;
                    mdl_Proyecciones_Venta viewfin = new mdl_Proyecciones_Venta();
                    viewfin.index = index;
                    viewfin.departamento = fin.departamento;
                    viewfin.concepto = fin.concepto;
                    viewfin.orden = 11;
                    viewfin.importe = fin.importe;
                    viewfin.por = viewfin.importe == 0 || ProyVentasNetas == 0 ? 0 :
                        Math.Round(viewfin.importe / ProyVentasNetas * 100, 2);
                    viewfin.proyimporte = importefinanciero is null ? 0 : importefinanciero.importe;
                    viewfin.proypor = viewfin.proyimporte == 0 || RealVentasNetas == 0 ? 0 :
                        Math.Round(viewfin.proyimporte / RealVentasNetas * 100, 2);

                    viewfin.indicador = ObtenerIndicador(false, viewfin.departamento, viewfin.importe, viewfin.proyimporte, viewfin.por, viewfin.proypor);
                    viewfin.diffimporte = viewfin.importe - viewfin.proyimporte;
                    viewfin.diffpor = viewfin.proyimporte == 0 || viewfin.diffimporte == 0 ? 0 :
                        Math.Round(viewfin.diffimporte / viewfin.proyimporte * 100, 2);

                    view.Add(viewfin);

                }

                double proyimportefinancieroOI = ProyFinancieros.Where(x => x.departamento == "OTROS INGRESOS").Sum(x => x.importe);
                double proyimportefinancieroOG = ProyFinancieros.Where(x => x.departamento == "OTROS GASTOS").Sum(x => x.importe);
                double realimportefinancieroOI = ERFinancieros.Where(x => x.departamento == "OTROS INGRESOS").Sum(x => x.importe);
                double realimportefinancieroOG = ERFinancieros.Where(x => x.departamento == "OTROS GASTOS").Sum(x => x.importe);

                index += 1;
                mdl_Proyecciones_Venta viewoi = new mdl_Proyecciones_Venta();
                viewoi.index = index;
                viewoi.departamento = "OTROS INGRESOS";
                viewoi.concepto = "Total Otros Ingresos";
                viewoi.orden = 11;
                viewoi.importe = proyimportefinancieroOI;
                viewoi.por = viewoi.importe == 0 || ProyVentasNetas == 0 ? 0 :
                    Math.Round(viewoi.importe / ProyVentasNetas * 100, 2);
                viewoi.proyimporte = realimportefinancieroOI;
                viewoi.proypor = viewoi.proyimporte == 0 || RealVentasNetas == 0 ? 0 :
                    Math.Round(viewoi.proyimporte / RealVentasNetas * 100, 2);
                viewoi.indicador = ObtenerIndicador(false, "Total Otros Ingresos", viewoi.importe, viewoi.proyimporte, viewoi.por, viewoi.proypor);
                viewoi.diffimporte = viewoi.importe - viewoi.proyimporte;
                viewoi.diffpor = viewoi.proyimporte == 0 || viewoi.diffimporte == 0 ? 0 :
                    Math.Round(viewoi.diffimporte / viewoi.proyimporte * 100, 2);
                view.Add(viewoi);

                index += 1;
                mdl_Proyecciones_Venta viewog = new mdl_Proyecciones_Venta();
                viewog.index = index;
                viewog.departamento = "OTROS GASTOS";
                viewog.concepto = "Total Otros Gastos";
                viewog.orden = 12;
                viewog.importe = proyimportefinancieroOG;
                viewog.por = viewog.importe == 0 || ProyVentasNetas == 0 ? 0 :
                    Math.Round(viewog.importe / ProyVentasNetas * 100, 2);
                viewog.proyimporte = realimportefinancieroOG;
                viewog.proypor = viewog.proyimporte == 0 || RealVentasNetas == 0 ? 0 :
                    Math.Round(viewog.proyimporte / RealVentasNetas * 100, 2);
                viewog.indicador = ObtenerIndicador(false, "Gastos de Departamento", viewog.importe, viewog.proyimporte, viewog.por, viewog.proypor);
                viewog.diffimporte = viewog.importe - viewog.proyimporte;
                viewog.diffpor = viewog.proyimporte == 0 || viewog.diffimporte == 0 ? 0 :
                    Math.Round(viewog.diffimporte / viewog.proyimporte * 100, 2);
                view.Add(viewog);

                index += 1;
                mdl_Proyecciones_Venta viewuo = new mdl_Proyecciones_Venta();
                viewuo.index = index;
                viewuo.departamento = "UTILIDAD ANTES DE IMPUESTOS";
                viewuo.concepto = "Utilidad";
                viewuo.orden = 13;
                viewuo.importe = proyOperacion + proyimportefinancieroOI - proyimportefinancieroOG;
                viewuo.por = viewuo.importe == 0 || ProyVentasNetas == 0 ? 0 :
                    Math.Round(viewuo.importe / ProyVentasNetas * 100, 2);
                viewuo.proyimporte = realOperacion + realimportefinancieroOI - realimportefinancieroOG;
                viewuo.proypor = viewuo.proyimporte == 0 || RealVentasNetas == 0 ? 0 :
                    Math.Round(viewuo.proyimporte / RealVentasNetas * 100, 2);
                viewuo.indicador = ObtenerIndicador(false, "Utilidad", viewuo.importe, viewuo.proyimporte, viewuo.por, viewuo.proypor);

                viewuo.diffimporte = viewuo.importe - viewuo.proyimporte;
                viewuo.diffpor = viewuo.proyimporte == 0 || viewuo.diffimporte == 0 ? 0 :
                    Math.Round(viewuo.diffimporte / viewuo.proyimporte * 100, 2);
                view.Add(viewuo);


                return view;

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string ObtenerIndicador(bool generado, string concepto, double importe, double proyimporte, double porcreal, double porcproy)
        {
            if (string.IsNullOrWhiteSpace(concepto))
                return "";

            string conceptoLower = concepto.ToLower();

            // 🔹 Si generado = true, filtrar conceptos
            if (generado)
            {
                bool aplicaIndicador =
                    conceptoLower.Contains("ventas netas") ||
                    conceptoLower.Contains("costo de venta") ||
                    conceptoLower.Contains("utilidad bruta") ||
                    conceptoLower.Contains("gastos") ||
                    conceptoLower.Contains("utilidad de operación");

                if (!aplicaIndicador)
                    return "";
            }

            // 🔹 Validación división
            if (proyimporte == 0)
                return "R";

            double porcentaje = (1 + (importe - proyimporte) / Math.Abs(proyimporte)) * 100;

            bool esVentasOGastos =
             conceptoLower.Contains("ventas netas") ||
             conceptoLower.Contains("gasto") ||
             conceptoLower.Contains("otros ingresos");
            if (esVentasOGastos)
            {
                if (conceptoLower.Contains("gasto", StringComparison.OrdinalIgnoreCase))
                {
                    if (importe < proyimporte) return "V";
                    //if (porcentaje <= 105) return "A";
                    return "R";
                }

                if (porcentaje > 85) return "V";
                if (porcentaje >= 60) return "A";
                return "R";
            }

            if (conceptoLower.Contains("costo"))
            {
                if (porcreal > porcproy + 1)
                    return "R";
                else if (porcreal > porcproy)
                    return "A";
                else
                    return "V";
            }

            if (porcreal < porcproy - 1)
                return "R";
            else if (porcreal < porcproy)
                return "A";
            else
                return "V";
        }
    }
}
