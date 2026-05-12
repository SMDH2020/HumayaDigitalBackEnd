using ClosedXML.Excel;
using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD_Auditoria.Consultas.Carga_Archivos
{
    public class AD_Carga_Inventario_Fisico
    {
        private string CadenaConexion;
        public AD_Carga_Inventario_Fisico(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Cargar_Inventario_Fisico_Response> Carga_Inventario_Fisico(mdl_Cargar_Inventario_Fisico mdl)
        {
            try
            {

                //Se construye un DataTable para pasarlo al TVP del stored
                var dt = new System.Data.DataTable();
                dt.Columns.Add("familia", typeof(string));
                dt.Columns.Add("codigo", typeof(string));
                dt.Columns.Add("descripcion", typeof(string));
                dt.Columns.Add("existencia_orig", typeof(float));
                dt.Columns.Add("unidad_medida", typeof(string));
                dt.Columns.Add("costo_unitario", typeof(float));
                dt.Columns.Add("pasillo", typeof(string));
                dt.Columns.Add("posicion", typeof(string));

                byte[] fileBytes = Convert.FromBase64String(mdl.documento);

                using (var stream = new MemoryStream(fileBytes))
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);

                    var rows = worksheet.RangeUsed().RowsUsed();

                    var lastRow = worksheet.LastRowUsed().RowNumber();

                    for (int i = 2; i <= lastRow; i++)
                    {
                        var row = worksheet.Row(i);

                        string sucursalExcel = row.Cell(4).GetValue<string>()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(sucursalExcel) && sucursalExcel != mdl.id_sucursal.ToString())
                        {
                            throw new Exception(
                                $"El archivo contiene sucursal {sucursalExcel}, pero la auditoría a la sucursal {mdl.id_sucursal.ToString()}."
                            );
                        }

                        var codigo = row.Cell(6).GetValue<string>()?.Trim() ?? "";
                        var descripcion = row.Cell(8).GetValue<string>()?.Trim() ?? "";
                        var posicion = row.Cell(9).GetValue<string>()?.Trim();

                        dt.Rows.Add(
                            row.Cell(5).GetValue<string>()?.Trim() ?? "",
                            codigo,
                            descripcion,
                            row.Cell(11).GetValue<float>(),
                            "PZ",
                            row.Cell(12).GetValue<float>(),
                            string.IsNullOrWhiteSpace(posicion)
                                ? "Des"
                                : posicion.PadRight(3).Substring(0, 3),
                            string.IsNullOrWhiteSpace(posicion)
                                ? "Desconocida"
                                : posicion
                        );
                    }
                }

                // EN CASO DE RECIBIR JSON
                //foreach (var item in mdl.inventario)
                //    dt.Rows.Add(item.familia, item.codigo, item.descripcion, item.existencia_orig, item.unidad_medida, item.costo_unitario, item.pasillo, item.posicion);

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@id_usuario", mdl.id_usuario, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);

                //TVP: DataTable construido anteriormente
                parametros.Add("@inventario", dt.AsTableValuedParameter("Auditoria.TVP_INVENTARIO"));

                //Parametros de respuesta
                parametros.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@ok", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@errores", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_INV_CARGAR_INVENTARIO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Cargar_Inventario_Fisico_Response
                {
                    Total = parametros.Get<int>("@total"),
                    Ok = parametros.Get<int>("@ok"),
                    Errores = parametros.Get<int>("@errores"),
                    Resultado = parametros.Get<int>("@resultado"),
                    Mensaje = parametros.Get<string>("@mensaje")
                };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
