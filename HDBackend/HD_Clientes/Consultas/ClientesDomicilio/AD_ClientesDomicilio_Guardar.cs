using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System.Data;

namespace HD.Clientes.Consultas.ClientesDomicilio
{
    public class AD_ClientesDomicilio_Guardar
    {
        private string CadenaConexion;
        public AD_ClientesDomicilio_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlClientesDomicilioList>> Guardar(mdlClienteDomicilioArray mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var tablaDomicilios = MapearDomiciliosADataTable(mdl.Domicilios);

                var parametros = new DynamicParameters();
                parametros.Add("idcliente", mdl.IdCliente);
                parametros.Add("usuario", mdl.usuario);
                parametros.Add("domicilios", tablaDomicilios.AsTableValuedParameter("Credito.TVP_ClientesDomicilio"));

                IEnumerable<mdlClientesDomicilioList> result = await factory.SQL.QueryAsync<mdlClientesDomicilioList>(
                    "Credito.sp_Clientes_Domicilio_Guardar_Array",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Mapea la lista de domicilios al DataTable que espera el TVP
        /// Credito.TipoClientesDomicilio. El orden y tipo de las columnas
        /// debe coincidir exactamente con la definición del tipo en SQL Server.
        /// </summary>
        private static DataTable MapearDomiciliosADataTable(List<MdlClientesDomicilioGuardar> domicilios)
        {
            var tabla = new DataTable();
            tabla.Columns.Add("orden", typeof(int));
            tabla.Columns.Add("idlocalidad", typeof(int));
            tabla.Columns.Add("direccion", typeof(string));
            tabla.Columns.Add("tipodomicilio", typeof(string));
            tabla.Columns.Add("principal", typeof(bool));
            tabla.Columns.Add("referencia1", typeof(string));
            tabla.Columns.Add("referencia2", typeof(string));
            tabla.Columns.Add("estatus", typeof(bool));
            tabla.Columns.Add("ubicacion", typeof(string));

            foreach (var d in domicilios)
            {
                tabla.Rows.Add(
                    d.orden,
                    d.idlocalidad,
                    d.direccion,
                    d.tipodomicilio,
                    d.principal,
                    d.referencia1 ?? (object)DBNull.Value,
                    d.referencia2 ?? (object)DBNull.Value,
                    d.estatus,
                    d.ubicacion ?? (object)DBNull.Value
                );
            }

            return tabla;
        }
    }
}
