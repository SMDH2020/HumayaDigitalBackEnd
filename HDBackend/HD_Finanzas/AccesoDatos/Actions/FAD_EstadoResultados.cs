using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Estado_Resultados;

namespace HD_Finanzas.AccesoDatos.Actions
{
    public class FAD_EstadoResultados
    {
        private string CadenaConexion;
        public FAD_EstadoResultados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<List<Fmdl_EstadoResultados_View>> GetEstadoResultadosByDireccionRolado(Fmdl_EstadoResultadosRolado vm, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    fechainicio = vm.fechainicio,
                    fechafin = vm.fechafin,
                    Departamentos = vm.departamento,
                    Sucursales = vm.sucursal,
                    ADR = vm.adr,
                    usuario = usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.SP_EstadoResultadosByDireccion_Rolado", parametros, commandType: System.Data.CommandType.StoredProcedure);

                List<Fmdl_EstadoResultados_Result> ERNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> ERLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();

                List<Fmdl_EstadoResultados_Result> PYNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> PYLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();

                List<Fmdl_EstadoResultados_Result> GstNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> GstLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();

                List<Fmdl_EstadoResultados_Result> ProyGstNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> ProyGstLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();

                List<Fmdl_EstadoResultados_Result> OGstNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> OGstLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();

                List<Fmdl_EstadoResultados_Result> ProyOGstNow = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                List<Fmdl_EstadoResultados_Result> ProyOGstLast = result.Read<Fmdl_EstadoResultados_Result>().ToList();
                factory.SQL.Close();
                List<Fmdl_EstadoResultados_View> view = new List<Fmdl_EstadoResultados_View>();
                int index = 0;
                foreach (Fmdl_EstadoResultados_Result er in ERNow)
                {
                    Fmdl_EstadoResultados_View obj = new Fmdl_EstadoResultados_View();
                    obj.index = index;
                    obj.concepto = er.concepto;
                    obj.departamento = er.departamento;
                    obj.importe = er.importe;
                    obj.orden = er.orden;
                    obj.por = er.por;

                    var objproy = PYNow.Where(x => x.departamento.Equals(er.departamento) && x.concepto.Equals(er.concepto)).FirstOrDefault();
                    obj.proyimporte = objproy is null ? 0 : Math.Round(objproy.importe, 2);
                    obj.proypor = objproy is null ? 0 : Math.Round(objproy.por, 2);

                    obj.diffimporte = Math.Round(obj.importe - obj.proyimporte, 2);
                    obj.diffpor = obj.diffimporte == 0 || obj.proyimporte == 0 ? 0
                        : Math.Round((obj.diffimporte / obj.proyimporte) * 100, 2);

                    var lastobj = ERLast.Where(x => x.departamento.Equals(er.departamento) && x.concepto.Equals(er.concepto)).FirstOrDefault();
                    obj.lastimporte = lastobj is null ? 0 : Math.Round(lastobj.importe, 2);
                    obj.lastpor = lastobj is null ? 0 : Math.Round(lastobj.por, 2);

                    //var lastproy = PYLast.Where(x => x.departamento.Equals(er.departamento) && x.concepto.Equals(er.concepto)).FirstOrDefault();
                    //obj.lastporyimporte = lastproy is null ? 0 : lastproy.importe;
                    //obj.lastproypor = lastproy is null ? 0 : lastproy.por;

                    //obj.lastdiffimporte = obj.lastimporte - obj.lastporyimporte;
                    //obj.lastdiffpor = obj.lastdiffimporte == 0 || obj.lastporyimporte == 0 ? 0
                    //    : (obj.lastdiffimporte / obj.lastporyimporte) * 100;

                    obj.lastdiffimporte = Math.Round(obj.importe - obj.lastimporte, 2);
                    obj.lastdiffpor = obj.lastdiffimporte == 0 || obj.lastimporte == 0 ? 0
                        : Math.Round((obj.lastdiffimporte / obj.lastimporte) * 100, 2);

                    view.Add(obj);
                    index++;
                }
                //var groups = view.GroupBy(x => x.departamento).ToList();
                //groups.ForEach(x =>
                //{
                //    var groupByDep = view.Where(item => item.departamento.Equals(x.Key)).ToList();
                //    var ventasNetas = groupByDep.Where(dif => dif.concepto.Equals("Ventas Netas")).FirstOrDefault();
                //    foreach (ViewER er in groupByDep)
                //    {
                //        view[er.index].diffpor = (er.diffimporte != 0 && ventasNetas.diffimporte != 0) 
                //        ? (er.diffimporte / ventasNetas.diffimporte) * 100 
                //        : 0;
                //        view[er.index].lastdiffpor = (er.lastdiffimporte != 0 && ventasNetas.lastdiffimporte != 0) ? Math.Round((er.lastdiffimporte / ventasNetas.lastdiffimporte) * 100, 2) : 0;
                //    }
                //});


                var VentasNetasTotales = view.Where(x => x.concepto.Equals("Ventas Netas")).Sum(x => x.importe);
                var ProyVentasNetasTotales = view.Where(x => x.concepto.Equals("Ventas Netas")).Sum(x => x.proyimporte);
                var VentasNetasTotalesLast = view.Where(x => x.concepto.Equals("Ventas Netas")).Sum(x => x.lastimporte);
                var ProyVentasNetasTotalesLast = view.Where(x => x.concepto.Equals("Ventas Netas")).Sum(x => x.lastporyimporte);

                var CostosVentasTotales = view.Where(x => x.orden == 4).Sum(x => x.importe);
                var ProyCostosVentasTotales = view.Where(x => x.orden == 4).Sum(x => x.proyimporte);
                var CostosVentasTotalesLast = view.Where(x => x.orden == 4).Sum(x => x.lastimporte);
                var ProyCostosVentasTotalesLast = view.Where(x => x.orden == 4).Sum(x => x.lastporyimporte);

                view.Add(new Fmdl_EstadoResultados_View
                {
                    index = index,
                    departamento = "VENTAS NETAS TOTALES",
                    concepto = "Ventas Totales",
                    orden = 10,
                    importe = Math.Round(VentasNetasTotales, 2),
                    por = 100,
                    proyimporte = Math.Round(ProyVentasNetasTotales, 2),
                    proypor = 100,
                    diffimporte = Math.Round(VentasNetasTotales - ProyVentasNetasTotales, 2),
                    diffpor = ((VentasNetasTotales - ProyVentasNetasTotales) == 0 || ProyVentasNetasTotales == 0) ? 0
                        : Math.Round(((VentasNetasTotales - ProyVentasNetasTotales) / ProyVentasNetasTotales) * 100, 2),
                    //diffpor =0,// Math.Round((VentasNetasTotales - ProyVentasNetasTotales) / ProyVentasNetasTotales, 2),
                    lastimporte = Math.Round(VentasNetasTotalesLast, 2),
                    lastpor = 100,
                    //lastporyimporte = ProyVentasNetasTotalesLast,
                    //lastproypor = 100,
                    //lastdiffimporte = VentasNetasTotalesLast - ProyVentasNetasTotalesLast,
                    //lastdiffpor = ((VentasNetasTotalesLast - ProyVentasNetasTotalesLast) == 0 || ProyVentasNetasTotalesLast == 0) ? 0
                    //: ((VentasNetasTotalesLast - ProyVentasNetasTotalesLast) / ProyVentasNetasTotalesLast) * 100
                    //lastdiffpor =0// Math.Round((VentasNetasTotalesLast - ProyVentasNetasTotalesLast) / ProyVentasNetasTotalesLast, 2)
                    lastdiffimporte = Math.Round(VentasNetasTotales - VentasNetasTotalesLast, 2),
                    lastdiffpor = ((VentasNetasTotales - VentasNetasTotalesLast) == 0 || VentasNetasTotalesLast == 0) ? 0
                    : Math.Round(((VentasNetasTotales - VentasNetasTotalesLast) / VentasNetasTotalesLast) * 100, 2)
                });
                index++;
                view.Add(new Fmdl_EstadoResultados_View
                {
                    index = index,
                    departamento = "VENTAS NETAS TOTALES",
                    concepto = "Costos de Venta",
                    orden = 10,
                    importe = Math.Round(CostosVentasTotales, 2),
                    por = (CostosVentasTotales == 0 || VentasNetasTotales == 0) ? 0
                    : Math.Round((CostosVentasTotales / VentasNetasTotales) * 100, 2),
                    proyimporte = Math.Round(ProyCostosVentasTotales, 2),
                    proypor = (ProyCostosVentasTotales == 0 || ProyVentasNetasTotales == 0) ? 0
                    : Math.Round((ProyCostosVentasTotales / ProyVentasNetasTotales) * 100, 2),
                    diffimporte = Math.Round(CostosVentasTotales - ProyCostosVentasTotales, 2),
                    diffpor = ((CostosVentasTotales - ProyCostosVentasTotales) == 0 || ProyCostosVentasTotales == 0) ? 0
                    : ((CostosVentasTotales - ProyCostosVentasTotales) / ProyCostosVentasTotales) * 100,
                    lastimporte = Math.Round(CostosVentasTotalesLast, 2),
                    lastpor = (CostosVentasTotalesLast == 0 || VentasNetasTotalesLast == 0) ? 0
                    : Math.Round((CostosVentasTotalesLast / VentasNetasTotalesLast) * 100, 2),
                    //lastporyimporte = ProyCostosVentasTotalesLast,
                    //lastproypor = 100,
                    //lastdiffimporte = CostosVentasTotalesLast - ProyCostosVentasTotalesLast,
                    //lastdiffpor = ((CostosVentasTotalesLast - ProyCostosVentasTotalesLast) == 0 || ProyCostosVentasTotalesLast == 0) ? 0
                    //: ((CostosVentasTotalesLast - ProyCostosVentasTotalesLast) / ProyCostosVentasTotalesLast) * 100
                    lastdiffimporte = Math.Round(CostosVentasTotales - CostosVentasTotalesLast, 2),
                    lastdiffpor = ((CostosVentasTotalesLast - CostosVentasTotales) == 0 || CostosVentasTotalesLast == 0) ? 0
                    : Math.Round(((CostosVentasTotales - CostosVentasTotalesLast) / CostosVentasTotalesLast) * 100, 2)
                });
                index++;
                var utilidadventas = VentasNetasTotales - CostosVentasTotales;
                var utilidadventaslast = VentasNetasTotalesLast - CostosVentasTotalesLast;
                var utilidadproyventas = ProyVentasNetasTotales - ProyCostosVentasTotales;
                var utilidadproyventaslast = ProyVentasNetasTotalesLast - ProyCostosVentasTotalesLast;
                var utilidadbrutaneta = new Fmdl_EstadoResultados_View
                {
                    index = index,
                    departamento = "VENTAS NETAS TOTALES",
                    concepto = "Utilidad Bruta",
                    orden = 10,
                    //Importe real del año
                    importe = Math.Round(utilidadventas, 2),
                    por = (utilidadventas == 0 || VentasNetasTotales == 0) ? 0 : Math.Round((utilidadventas / VentasNetasTotales) * 100, 2),

                    //Importe proyectado del año
                    proyimporte = Math.Round(utilidadproyventas, 2),
                    proypor = (utilidadproyventas == 0 || ProyVentasNetasTotales == 0) ? 0 : Math.Round((utilidadproyventas / ProyVentasNetasTotales) * 100, 2),

                    //diferencia de lo real menos lo proyectado
                    diffimporte = Math.Round((utilidadventas - utilidadproyventas), 2),
                    diffpor = ((utilidadventas - utilidadproyventas) == 0 || utilidadproyventas == 0) ? 0 : Math.Round(((utilidadventas - utilidadproyventas) / utilidadproyventas) * 100, 2),

                    //Importe real del año anterioro
                    lastimporte = Math.Round(utilidadventaslast, 2),
                    lastpor = (utilidadventaslast == 0 || VentasNetasTotalesLast == 0) ? 0 : Math.Round((utilidadventaslast / VentasNetasTotalesLast) * 100, 2),

                    //lastporyimporte = utilidadproyventaslast,
                    //lastproypor = (utilidadproyventaslast == 0 || ProyVentasNetasTotalesLast == 0) ? 0 : (utilidadproyventaslast / ProyVentasNetasTotalesLast) * 100,
                    //lastdiffimporte = utilidadventaslast - utilidadproyventaslast,
                    //lastdiffpor = ((utilidadventaslast - utilidadproyventaslast) == 0 || utilidadproyventaslast == 0) ? 0 : ((utilidadventaslast - utilidadproyventaslast) / utilidadproyventaslast) * 100

                    //Diferencia de lo real del año actual menos lo real del año anterior
                    lastdiffimporte = Math.Round(utilidadventas - utilidadventaslast, 2),
                    lastdiffpor = ((utilidadventaslast - utilidadventas) == 0 || utilidadventaslast == 0) ? 0 : Math.Round(((utilidadventas - utilidadventaslast) / utilidadventaslast) * 100, 2)
                };
                view.Add(utilidadbrutaneta);
                index++;

                Fmdl_EstadoResultados_View viewdepartamentos = new Fmdl_EstadoResultados_View();
                viewdepartamentos.index = index;
                viewdepartamentos.departamento = "VENTAS NETAS TOTALES";
                viewdepartamentos.concepto = "Gastos departamentales";

                double gstDepartamento = ERNow.Where(x => x.concepto.Equals("Gastos de Departamento")).Sum(x => x.importe);
                double gstDepartamentoLast = ERLast.Where(x => x.concepto.Equals("Gastos de Departamento")).Sum(x => x.importe);
                double gstProy = PYNow.Where(x => x.concepto.Equals("Gastos de Departamento")).Sum(x => x.importe);
                double gstProyLast = PYLast.Where(x => x.concepto.Equals("Gastos de Departamento")).Sum(x => x.importe);

                viewdepartamentos.importe = Math.Round(gstDepartamento, 2);
                viewdepartamentos.por = (VentasNetasTotales == 0 || gstDepartamento == 0) ? 0
                    : Math.Round((gstDepartamento / VentasNetasTotales) * 100, 2);

                viewdepartamentos.proyimporte = Math.Round(gstProy, 2);
                viewdepartamentos.proypor = (ProyVentasNetasTotales == 0 || gstProy == 0) ? 0
                    : Math.Round((gstProy / ProyVentasNetasTotales) * 100, 2);

                viewdepartamentos.diffimporte = Math.Round(gstDepartamento - gstProy, 2);
                viewdepartamentos.diffpor = (gstDepartamento - gstProy) == 0 || ProyVentasNetasTotales == 0 ? 0
                    : Math.Round(((gstDepartamento - gstProy) / ProyVentasNetasTotales) * 100, 2);

                viewdepartamentos.lastimporte = Math.Round(gstDepartamentoLast, 2);
                viewdepartamentos.lastpor = VentasNetasTotalesLast == 0 || gstDepartamentoLast == 0 ? 0
                    : Math.Round((gstDepartamentoLast / VentasNetasTotalesLast) * 100, 2);

                //viewdepartamentos.lastporyimporte = gstProyLast;
                //viewdepartamentos.lastproypor = gstProyLast == 0 || ProyVentasNetasTotalesLast == 0 ? 0
                //    : (gstProyLast / ProyVentasNetasTotalesLast) * 100;

                //viewdepartamentos.lastdiffimporte = gstDepartamentoLast - gstProyLast;
                //viewdepartamentos.lastdiffpor = (gstDepartamentoLast - gstProyLast) == 0 || ProyVentasNetasTotalesLast == 0 ? 0
                //    : ((gstDepartamentoLast - gstProyLast) / ProyVentasNetasTotalesLast) * 100;
                viewdepartamentos.lastdiffimporte = Math.Round(gstDepartamento - gstDepartamentoLast, 2);
                viewdepartamentos.lastdiffpor = (gstDepartamentoLast - gstDepartamento) == 0 || gstDepartamentoLast == 0 ? 0
                    : Math.Round(((gstDepartamento - gstDepartamentoLast) / gstDepartamentoLast) * 100, 2);

                view.Add(viewdepartamentos);

                index++;
                foreach (Fmdl_EstadoResultados_Result erg in GstNow)
                {
                    Fmdl_EstadoResultados_View newitem = new Fmdl_EstadoResultados_View();
                    newitem.index = index;
                    newitem.departamento = "VENTAS NETAS TOTALES";
                    newitem.concepto = "Gastos de " + erg.concepto.ToLower();
                    newitem.importe = Math.Round(erg.importe, 2);
                    newitem.por = (erg.importe == 0 || VentasNetasTotales == 0) ? 0 : Math.Round((erg.importe / VentasNetasTotales) * 100, 2);
                    var resultProyGstNow = ProyGstNow.Where(x => x.departamento.Equals(erg.departamento) && x.concepto.Equals(erg.concepto)).FirstOrDefault();
                    if (resultProyGstNow is null)
                    {
                        newitem.proyimporte = 0;
                        newitem.proypor = 0;
                    }
                    else
                    {
                        newitem.proyimporte = Math.Round(resultProyGstNow.importe, 2);
                        newitem.proypor = (resultProyGstNow.importe == 0 || ProyVentasNetasTotales == 0) ? 0
                            : Math.Round((resultProyGstNow.importe / ProyVentasNetasTotales) * 100, 2);
                    }

                    var resultGstLast = GstLast.Where(x => x.departamento.Equals(erg.departamento) && x.concepto.Equals(erg.concepto)).FirstOrDefault();
                    if (resultGstLast is null)
                    {
                        newitem.lastimporte = 0;
                        newitem.lastpor = 0;
                    }
                    else
                    {
                        newitem.lastimporte = Math.Round(resultGstLast.importe, 2);
                        newitem.lastpor = (resultGstLast.importe == 0 || VentasNetasTotalesLast == 0) ? 0
                            : Math.Round((resultGstLast.importe / VentasNetasTotalesLast) * 100, 2);
                    }

                    //var resultProyGstLast = ProyGstLast.Where(x => x.departamento.Equals(erg.departamento) && x.concepto.Equals(erg.concepto)).FirstOrDefault();
                    //if (resultProyGstLast is null)
                    //{
                    //    newitem.lastporyimporte = 0;
                    //    newitem.lastproypor = 0;
                    //}
                    //else
                    //{
                    //    newitem.lastporyimporte = resultProyGstLast.importe;
                    //    newitem.lastproypor = (resultProyGstLast.importe == 0 || ProyVentasNetasTotalesLast == 0) ? 0
                    //        : (resultProyGstLast.importe / ProyVentasNetasTotalesLast) * 100;
                    //}
                    index += 1;

                    newitem.diffimporte = Math.Round(newitem.importe - newitem.proyimporte, 2);
                    newitem.diffpor = newitem.diffimporte == 0 || newitem.proyimporte == 0 ? 0
                        : Math.Round((newitem.diffimporte / newitem.proyimporte) * 100, 2);

                    newitem.lastdiffimporte = Math.Round(newitem.importe - newitem.lastimporte, 2);
                    newitem.lastdiffpor = newitem.lastdiffimporte == 0 || newitem.lastimporte == 0 ? 0
                        : Math.Round((newitem.lastdiffimporte / newitem.lastimporte) * 100, 2);

                    view.Add(newitem);
                }
                Fmdl_EstadoResultados_View viewutilidad = new Fmdl_EstadoResultados_View();

                double utilope = utilidadventas - gstDepartamento - GstNow.Sum(x => x.importe);
                double utilproy = utilidadproyventas - gstProy - ProyGstNow.Sum(x => x.importe);
                double utilopelast = utilidadventaslast - gstDepartamentoLast - GstLast.Sum(x => x.importe);
                double utilproylast = utilidadproyventaslast - gstProyLast - ProyGstLast.Sum(x => x.importe);

                viewutilidad.index = index;
                viewutilidad.departamento = "VENTAS NETAS TOTALES";
                viewutilidad.concepto = "Utilidad de Operación";
                viewutilidad.importe = Math.Round(utilope, 2);
                viewutilidad.por = utilope == 0 || VentasNetasTotales == 0 ? 0 : Math.Round((utilope / VentasNetasTotales) * 100, 2);
                viewutilidad.proyimporte = Math.Round(utilproy);
                viewutilidad.proypor = utilproy == 0 || ProyVentasNetasTotales == 0 ? 0 : Math.Round((utilproy / ProyVentasNetasTotales) * 100, 2);
                viewutilidad.diffimporte = Math.Round(utilope - utilproy, 2);
                viewutilidad.diffpor = viewutilidad.diffimporte == 0 || utilproy == 0 ? 0 : Math.Round((viewutilidad.diffimporte / utilproy) * 100, 2);
                viewutilidad.lastimporte = Math.Round(utilopelast);
                viewutilidad.lastpor = utilopelast == 0 || VentasNetasTotalesLast == 0 ? 0 : Math.Round((utilopelast / VentasNetasTotalesLast) * 100, 2);
                //viewutilidad.lastporyimporte = utilproylast;
                //viewutilidad.lastproypor = utilproylast == 0 || ProyVentasNetasTotalesLast == 0 ? 0 : (utilproylast / ProyVentasNetasTotalesLast) * 100;
                viewutilidad.lastdiffimporte = Math.Round(utilope - viewutilidad.lastimporte, 2);
                viewutilidad.lastdiffpor = viewutilidad.lastdiffimporte == 0 || utilopelast == 0 ? 0 : Math.Round((viewutilidad.lastdiffimporte / utilopelast) * 100, 2);
                view.Add(viewutilidad);

                index++;
                //var otrosingresos = OGstNow.Where(x => x.departamento.Equals("OTROS INGRESOS")).ToList();
                OGstNow.ForEach(item =>
                {
                    Fmdl_EstadoResultados_View oingresos = new Fmdl_EstadoResultados_View();
                    oingresos.index = index;
                    oingresos.departamento = item.departamento;
                    oingresos.concepto = item.concepto;
                    oingresos.importe = Math.Round(item.importe, 2);
                    oingresos.por = oingresos.importe == 0 || VentasNetasTotales == 0 ? 0 : Math.Round((oingresos.importe / VentasNetasTotales) * 100, 2);

                    var oproyingresos = ProyOGstNow.Where(x => x.departamento.Equals(item.departamento) && x.concepto.Equals(item.concepto)).FirstOrDefault();
                    if (oproyingresos is null)
                    {
                        oingresos.proyimporte = 0;
                        oingresos.proypor = 0;
                    }
                    else
                    {
                        oingresos.proyimporte = Math.Round(oproyingresos.importe, 2);
                        oingresos.proypor = oingresos.proyimporte == 0 || ProyVentasNetasTotales == 0 ? 0 : Math.Round((oingresos.proyimporte / ProyVentasNetasTotales) * 100, 2);
                    }

                    var oingresoslast = OGstLast.Where(x => x.departamento.Equals(item.departamento) && x.concepto.Equals(item.concepto)).FirstOrDefault();
                    if (oingresoslast is null)
                    {
                        oingresos.lastimporte = 0;
                        oingresos.lastpor = 0;
                    }
                    else
                    {
                        oingresos.lastimporte = Math.Round(oingresoslast.importe, 2);
                        oingresos.lastpor = oingresos.lastimporte == 0 || VentasNetasTotalesLast == 0 ? 0 : Math.Round((oingresos.lastimporte / VentasNetasTotalesLast) * 100, 2);
                    }

                    //var oproyingresoslast = ProyOGstLast.Where(x => x.departamento.Equals(item.departamento) && x.concepto.Equals(item.concepto)).FirstOrDefault();
                    //if (oproyingresoslast is null)
                    //{
                    //    oingresos.lastporyimporte = 0;
                    //    oingresos.lastproypor = 0;
                    //}
                    //else
                    //{
                    //    oingresos.lastporyimporte = oproyingresoslast.importe;
                    //    oingresos.lastproypor = oingresos.lastporyimporte == 0 || ProyVentasNetasTotalesLast == 0 ? 0 : (oingresos.lastporyimporte / ProyVentasNetasTotalesLast) * 100;
                    //}

                    oingresos.diffimporte = Math.Round(oingresos.importe - oingresos.proyimporte, 2);
                    oingresos.diffpor = oingresos.diffimporte == 0 || oingresos.proyimporte == 0 ? 0 : Math.Round((oingresos.diffimporte / oingresos.proyimporte) * 100, 2);

                    oingresos.lastdiffimporte = Math.Round(oingresos.importe - oingresos.lastimporte, 2);
                    oingresos.lastdiffpor = oingresos.lastdiffimporte == 0 || oingresos.lastimporte == 0 ? 0 : Math.Round((oingresos.lastdiffimporte / oingresos.lastimporte) * 100, 2);


                    view.Add(oingresos);
                });

                index++;
                var totalotrosingresos = new Fmdl_EstadoResultados_View();
                totalotrosingresos.index = index;
                totalotrosingresos.departamento = "OTROS INGRESOS";
                totalotrosingresos.concepto = "Total Otros Ingresos";
                totalotrosingresos.importe = Math.Round(OGstNow.Where(x => x.departamento.Equals("OTROS INGRESOS")).Sum(x => x.importe), 2);
                totalotrosingresos.por = totalotrosingresos.importe == 0 || VentasNetasTotales == 0 ? 0 : Math.Round((totalotrosingresos.importe / VentasNetasTotales) * 100, 2);
                totalotrosingresos.proyimporte = Math.Round(ProyOGstNow.Where(x => x.departamento.Equals("OTROS INGRESOS")).Sum(x => x.importe), 2);
                totalotrosingresos.proypor = totalotrosingresos.proyimporte == 0 || ProyVentasNetasTotales == 0 ? 0 : Math.Round((totalotrosingresos.proyimporte / ProyVentasNetasTotales) * 100, 2);
                totalotrosingresos.lastimporte = Math.Round(OGstLast.Where(x => x.departamento.Equals("OTROS INGRESOS")).Sum(x => x.importe), 2);
                totalotrosingresos.lastpor = totalotrosingresos.lastimporte == 0 || VentasNetasTotalesLast == 0 ? 0 : Math.Round((totalotrosingresos.lastimporte / VentasNetasTotalesLast) * 100, 2);
                //totalotrosingresos.lastporyimporte = ProyOGstLast.Where(x => x.departamento.Equals("OTROS INGRESOS")).Sum(x => x.importe);
                //totalotrosingresos.lastproypor = totalotrosingresos.lastporyimporte == 0 || ProyVentasNetasTotalesLast == 0 ? 0 : (totalotrosingresos.lastporyimporte / ProyVentasNetasTotalesLast) * 100;
                totalotrosingresos.diffimporte = Math.Round(totalotrosingresos.importe - totalotrosingresos.proyimporte, 2);
                totalotrosingresos.diffpor = totalotrosingresos.diffimporte == 0 || totalotrosingresos.proyimporte == 0 ? 0
                    : Math.Round((totalotrosingresos.diffimporte / totalotrosingresos.proyimporte) * 100, 2);
                //totalotrosingresos.lastdiffimporte = totalotrosingresos.lastimporte - totalotrosingresos.lastporyimporte;
                //totalotrosingresos.lastdiffpor = totalotrosingresos.lastdiffimporte == 0 || totalotrosingresos.lastporyimporte == 0 ? 0
                //    : (totalotrosingresos.lastdiffimporte / totalotrosingresos.lastporyimporte) * 100;
                totalotrosingresos.lastdiffimporte = Math.Round(totalotrosingresos.importe - totalotrosingresos.lastimporte, 2);
                totalotrosingresos.lastdiffpor = totalotrosingresos.lastdiffimporte == 0 || totalotrosingresos.lastimporte == 0 ? 0
                    : Math.Round((totalotrosingresos.lastdiffimporte / totalotrosingresos.lastimporte) * 100, 2);
                view.Add(totalotrosingresos);
                index++;

                var totalotrosgastos = new Fmdl_EstadoResultados_View();
                totalotrosgastos.index = index;
                totalotrosgastos.departamento = "OTROS GASTOS";
                totalotrosgastos.concepto = "Total Otros Gastos";
                totalotrosgastos.importe = Math.Round(OGstNow.Where(x => x.departamento.Equals("OTROS GASTOS")).Sum(x => x.importe), 2);
                totalotrosgastos.por = totalotrosgastos.importe == 0 || VentasNetasTotales == 0 ? 0 : Math.Round((totalotrosgastos.importe / VentasNetasTotales) * 100, 2);
                totalotrosgastos.proyimporte = Math.Round(ProyOGstNow.Where(x => x.departamento.Equals("OTROS GASTOS")).Sum(x => x.importe), 2);
                totalotrosgastos.proypor = totalotrosgastos.proyimporte == 0 || ProyVentasNetasTotales == 0 ? 0 : Math.Round((totalotrosgastos.proyimporte / ProyVentasNetasTotales) * 100, 2);
                totalotrosgastos.lastimporte = Math.Round(OGstLast.Where(x => x.departamento.Equals("OTROS GASTOS")).Sum(x => x.importe), 2);
                totalotrosgastos.lastpor = totalotrosgastos.lastimporte == 0 || VentasNetasTotalesLast == 0 ? 0 : Math.Round((totalotrosgastos.lastimporte / VentasNetasTotalesLast) * 100, 2);
                //totalotrosgastos.lastporyimporte = ProyOGstLast.Where(x => x.departamento.Equals("OTROS GASTOS")).Sum(x => x.importe);
                //totalotrosgastos.lastproypor = totalotrosgastos.lastporyimporte == 0 || ProyVentasNetasTotalesLast == 0 ? 0 : (totalotrosgastos.lastporyimporte / ProyVentasNetasTotalesLast) * 100; ;


                totalotrosgastos.diffimporte = Math.Round(totalotrosgastos.importe - totalotrosgastos.proyimporte, 2);
                totalotrosgastos.diffpor = totalotrosgastos.diffimporte == 0 || totalotrosgastos.proyimporte == 0 ? 0
                    : Math.Round((totalotrosgastos.diffimporte / totalotrosgastos.proyimporte) * 100, 2);
                totalotrosgastos.lastdiffimporte = Math.Round(totalotrosgastos.importe - totalotrosgastos.lastimporte, 2);
                totalotrosgastos.lastdiffpor = totalotrosgastos.lastdiffimporte == 0 || totalotrosgastos.lastimporte == 0 ? 0
                    : Math.Round((totalotrosgastos.lastdiffimporte / totalotrosgastos.lastimporte) * 100, 2);

                view.Add(totalotrosgastos);


                var utilidadantesimpuestos = new Fmdl_EstadoResultados_View();
                utilidadantesimpuestos.index = index;
                utilidadantesimpuestos.departamento = "UTILIDAD ANTES DE IMPUESTOS";
                utilidadantesimpuestos.concepto = "Utilidad";
                utilidadantesimpuestos.importe = Math.Round(viewutilidad.importe + totalotrosingresos.importe - totalotrosgastos.importe, 2);
                utilidadantesimpuestos.por = utilidadantesimpuestos.importe == 0 || VentasNetasTotales == 0 ? 0
                    : Math.Round((utilidadantesimpuestos.importe / VentasNetasTotales) * 100, 2);

                utilidadantesimpuestos.proyimporte = Math.Round(viewutilidad.proyimporte + totalotrosingresos.proyimporte - totalotrosgastos.proyimporte, 2);
                utilidadantesimpuestos.proypor = utilidadantesimpuestos.proyimporte == 0 || ProyVentasNetasTotales == 0 ? 0
                    : Math.Round((utilidadantesimpuestos.proyimporte / ProyVentasNetasTotales) * 100, 2);


                utilidadantesimpuestos.diffimporte = Math.Round(utilidadantesimpuestos.importe - utilidadantesimpuestos.proyimporte, 2);
                //utilidadantesimpuestos.diffimporte =  viewutilidad.diffimporte + totalotrosingresos.diffimporte - totalotrosgastos.diffimporte;
                utilidadantesimpuestos.diffpor = utilidadantesimpuestos.diffimporte == 0 || utilidadantesimpuestos.proyimporte == 0 ? 0
                    : Math.Round((utilidadantesimpuestos.diffimporte / utilidadantesimpuestos.proyimporte) * 100, 2);


                utilidadantesimpuestos.lastimporte = Math.Round(viewutilidad.lastimporte + totalotrosingresos.lastimporte - totalotrosgastos.lastimporte, 2);
                utilidadantesimpuestos.lastpor = utilidadantesimpuestos.lastimporte == 0 || VentasNetasTotalesLast == 0 ? 0
                    : Math.Round((utilidadantesimpuestos.lastimporte / VentasNetasTotalesLast) * 100, 2);

                //utilidadantesimpuestos.lastporyimporte = viewutilidad.lastporyimporte + totalotrosingresos.lastporyimporte - totalotrosgastos.lastporyimporte;
                //utilidadantesimpuestos.lastproypor = utilidadantesimpuestos.lastporyimporte == 0 || ProyVentasNetasTotalesLast == 0 ? 0
                //    : (utilidadantesimpuestos.lastporyimporte / ProyVentasNetasTotalesLast) * 100;


                //utilidadantesimpuestos.lastdiffimporte = viewutilidad.lastdiffimporte + totalotrosingresos.lastdiffimporte - totalotrosgastos.lastdiffimporte;
                //utilidadantesimpuestos.lastdiffimporte = utilidadantesimpuestos.lastimporte - utilidadantesimpuestos.lastporyimporte;
                //utilidadantesimpuestos.lastdiffpor = utilidadantesimpuestos.lastdiffimporte == 0 || utilidadantesimpuestos.lastporyimporte == 0 ? 0
                //    : (utilidadantesimpuestos.lastdiffimporte / utilidadantesimpuestos.lastporyimporte) * 100;

                utilidadantesimpuestos.lastdiffimporte = Math.Round(utilidadantesimpuestos.importe - utilidadantesimpuestos.lastimporte, 2);
                utilidadantesimpuestos.lastdiffpor = utilidadantesimpuestos.lastdiffimporte == 0 || utilidadantesimpuestos.lastimporte == 0 ? 0
                    : Math.Round((utilidadantesimpuestos.lastdiffimporte / utilidadantesimpuestos.lastimporte) * 100, 2);

                view.Add(utilidadantesimpuestos);
                return view;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
