using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.CRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM
{
    public class AD_Datos_Buro_Credito_Guardar
    {
        private string CadenaConexion;
        public AD_Datos_Buro_Credito_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Datos_Buro_Credito_View> Guardar(mdl_DatosBuroCredito mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var tablaDomicilios = MapearDomiciliosADataTable(mdl.Domicilios);

                var parametros = new DynamicParameters();
                parametros.Add("idcliente", mdl.IdCliente);
                parametros.Add("usuario", mdl.usuario);
                parametros.Add("domicilios", tablaDomicilios.AsTableValuedParameter("Credito.TVP_ClientesDomicilio"));
                parametros.Add("buro_credito", mdl.buro_credito);

                parametros.Add("nombre", mdl.nombre);
                parametros.Add("nombre2", mdl.nombre2);
                parametros.Add("apellido_paterno", mdl.apellido_paterno);
                parametros.Add("apellido_materno", mdl.apellido_materno);
                parametros.Add("curp", mdl.curp);
                parametros.Add("sexo", mdl.sexo);
                parametros.Add("estado_civil", mdl.estado_civil);
                parametros.Add("regimen_conyugal", mdl.regimen_conyugal);
                parametros.Add("tipo_persona", mdl.tipoPersona);


                using (var multi = await factory.SQL.QueryMultipleAsync(
                    "Credito.sp_Datos_Buro_Credito_Guardar",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure))
                {
                    var resultado = new mdl_Datos_Buro_Credito_View
                    {
                        datos_persona_fisica = (await multi.ReadAsync<mdlClientes_Datos_Persona_Fisica>()).FirstOrDefault(),
                        domicilios = (await multi.ReadAsync<mdlClientesDomicilioList>()).ToList()
                    };
                    factory.SQL.Close();
                    return resultado;
                }
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
