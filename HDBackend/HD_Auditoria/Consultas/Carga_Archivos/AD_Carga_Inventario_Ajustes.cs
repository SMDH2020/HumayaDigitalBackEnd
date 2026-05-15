using ClosedXML.Excel;
using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;
using System.Globalization;

namespace HD_Auditoria.Consultas.Carga_Archivos
{
    public class AD_Carga_Inventario_Ajustes
    {
        private string CadenaConexion;
        public AD_Carga_Inventario_Ajustes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Cargar_Inventario_Ajustes_Response> Carga_Inventario_Ajustes(mdl_Cargar_Inventario_Ajustes mdl)
        {
            try
            {
                //Se construye un DataTable para pasarlo al TVP del stored
                var dt = new System.Data.DataTable();
                dt.Columns.Add("codigo", typeof(string));
                dt.Columns.Add("descripcion", typeof(string));
                dt.Columns.Add("cantidad", typeof(float));
                dt.Columns.Add("sucursal_origen", typeof(string));
                dt.Columns.Add("sucursal_dest", typeof(string));
                dt.Columns.Add("fecha_envio", typeof(string));
                dt.Columns.Add("referencia_doc", typeof(string));

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

                        // =========================
                        // TRANSITO
                        // =========================
                        if (mdl.tipo_ajuste == "T")
                        {
                            // VALIDAR ENCABEZADOS
                            var headers = worksheet.Row(1);

                            string hCodigo = headers.Cell(13).GetValue<string>()?.Trim();
                            string hDescripcion = headers.Cell(14).GetValue<string>()?.Trim();
                            string hCantidad = headers.Cell(16).GetValue<string>()?.Trim();
                            string hSucursalO = headers.Cell(6).GetValue<string>()?.Trim();
                            string hSucursalD = headers.Cell(9).GetValue<string>()?.Trim();
                            string hFecha = headers.Cell(12).GetValue<string>()?.Trim();

                            if (
                                !hCodigo.ToLower().Contains("v_part_no") ||
                                !hDescripcion.ToLower().Contains("v_part_desc") ||
                                !hCantidad.ToLower().Contains("v_enviada") ||
                                !hSucursalO.ToLower().Contains("v_sucursal") ||
                                !hSucursalD.ToLower().Contains("v_cliente") ||
                                !hFecha.ToLower().Contains("v_fecha_mov")
                            )
                            {
                                throw new Exception("El archivo no corresponde a un formato de INVENTARIO EN TRANSITO válido.");
                            }

                            string sucursalExcel = System.Text.RegularExpressions.Regex.Match(
                                    row.Cell(6).GetValue<string>() ?? "",
                                    @"\((\d+)\)"
                                ).Groups[1].Value ?? "";

                            if (!string.IsNullOrEmpty(sucursalExcel) && sucursalExcel != mdl.id_sucursal.ToString())
                            {
                                throw new Exception(
                                    $"El archivo contiene sucursal {sucursalExcel}, pero la auditoría es de la sucursal {mdl.id_sucursal.ToString()}."
                                );
                            }

                            float cantidad = 0;

                            var valor = row.Cell(16).GetValue<string>();

                            if (!string.IsNullOrWhiteSpace(valor))
                                float.TryParse(valor, out cantidad);

                            DateTime fechaEnvio;

                            var cell = row.Cell(12);

                            if (cell.DataType == XLDataType.DateTime)
                            {
                                fechaEnvio = cell.GetDateTime();
                            }
                            else
                            {
                                DateTime.TryParse(cell.GetValue<string>()?.Trim(), out fechaEnvio);
                            }

                            string fechaFormateada = fechaEnvio == DateTime.MinValue
                                ? ""
                                : fechaEnvio.ToString("yyyy/MM/dd");



                            dt.Rows.Add(
                                row.Cell(13).GetValue<string>()?.Trim() ?? "", // codigo
                                row.Cell(14).GetValue<string>()?.Trim() ?? "", // descripcion
                                cantidad,                                       // cantidad
                                System.Text.RegularExpressions.Regex.Match(
                                    row.Cell(6).GetValue<string>() ?? "",
                                    @"\((\d+)\)"
                                ).Groups[1].Value,                               // sucursal_origen

                                System.Text.RegularExpressions.Regex.Match(
                                    row.Cell(9).GetValue<string>() ?? "",
                                    @"\((\d+)\)"
                                ).Groups[1].Value,                              // sucursal_dest
                                fechaFormateada, // fecha_envio
                                ""                                             // referencia_doc
                            );
                        }

                        // =========================
                        // SURTIDO
                        // =========================
                        else if (mdl.tipo_ajuste == "S")
                        {

                            // VALIDAR ENCABEZADOS
                            var headers = worksheet.Row(1);

                            string hCodigo = headers.Cell(17).GetValue<string>()?.Trim();
                            string hDescripcion = headers.Cell(18).GetValue<string>()?.Trim();
                            string hCantidad = headers.Cell(20).GetValue<string>()?.Trim();
                            string hDocumento = headers.Cell(36).GetValue<string>()?.Trim();

                            if (
                                !hCodigo.ToLower().Contains("v_part_no") ||
                                !hDescripcion.ToLower().Contains("v_part_desc") ||
                                !hCantidad.ToLower().Contains("v_qty_sup") ||
                                !hDocumento.ToLower().Contains("v_ot")
                            )
                            {
                                throw new Exception("El archivo no corresponde a un formato de INVENTARIO SURTIDO válido.");
                            }

                            string sucursalExcel = System.Text.RegularExpressions.Regex.Match(
                                    row.Cell(6).GetValue<string>() ?? "",
                                    @"\((\d+)\)"
                                ).Groups[1].Value ?? "";

                            if (!string.IsNullOrEmpty(sucursalExcel) && sucursalExcel != mdl.id_sucursal.ToString())
                            {
                                throw new Exception(
                                    $"El archivo contiene sucursal {sucursalExcel}, pero la auditoría es de la sucursal {mdl.id_sucursal.ToString()}."
                                );
                            }

                            dt.Rows.Add(
                                row.Cell(17).GetValue<string>()?.Trim() ?? "", // codigo
                                row.Cell(18).GetValue<string>()?.Trim() ?? "", // descripcion
                                row.Cell(20).GetValue<float>(),                // cantidad
                                "",                                             // sucursal_origen
                                "",                                             // sucursal_dest
                                "",                                             // fecha_envio
                                row.Cell(36).GetValue<string>()?.Trim() ?? ""   // referencia_doc
                            );
                        }
                    }
                }

                //foreach (var item in mdl.ajustes)
                //    dt.Rows.Add(item.codigo, item.descripcion, item.cantidad, item.sucursal_origen, item.sucursal_dest, item.fecha_envio, item.referencia_doc);

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@tipo_ajuste", mdl.tipo_ajuste, System.Data.DbType.String, System.Data.ParameterDirection.Input, 1);
                parametros.Add("@id_usuario", mdl.id_usuario, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);

                //TVP: DataTable construido anteriormente
                parametros.Add("@ajustes", dt.AsTableValuedParameter("Auditoria.TVP_AJUSTES"));

                //Parametros de respuesta
                parametros.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@ok", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@errores", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_INV_CARGAR_AJUSTES", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Cargar_Inventario_Ajustes_Response
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
