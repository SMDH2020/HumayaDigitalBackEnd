using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.CRM;

namespace HD.Clientes.Consultas.CRM
{
    public class AD_Datos_CRM
    {
        private string CadenaConexion;
        public AD_Datos_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Clientes_CRM>> Listado(string? usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_CRM> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_CRM>("CRM.sp_clientes_dropdownlist", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Info_Cliente_CRM_ID_View> Obtener_Info_Cliente(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Get_Info_Cliente_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Info_Cliente_CRM_ID_View mdl = new mdl_Info_Cliente_CRM_ID_View();
                mdl.info_General_cliente = result.Read<mdl_Info_Cliente_CRM>().FirstOrDefault();
                mdl.opciones_estatus = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_origen = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_tipo = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_clasificacion = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_superficie = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_tecnologia = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_etiqueta = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_cultivo_terreno = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_cultivo_riego = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.info_ubicacion_cliente = result.Read<mdl_Info_Cliente_Ubicacion_CRM>().ToList();
                mdl.opciones_estado = result.Read<mdl_Opciones_Estado_CRM>().ToList();
                mdl.opciones_municipio = result.Read<mdl_Opciones_Municipio_CRM>().ToList();
                mdl.info_Facturacion_cliente = result.Read<mdl_Info_Cliente_Facturacion_CRM>().FirstOrDefault();
                mdl.opciones_lineas = result.Read<mdl_Opciones_Lineas_CRM>().ToList();
                mdl.opciones_giros = result.Read<mdl_Opciones_Giros_CRM>().ToList();
                mdl.info_clasificacion_cliente = result.Read<mdl_Info_Cliente_Clasificación_CRM>().FirstOrDefault();
                mdl.opciones_asesor = result.Read<mdl_Opciones_Asesor>().ToList();
                mdl.info_asesores_cliente = result.Read<mdl_Info_Cliente_Asesores_CRM>().ToList();
                mdl.info_cultivo_cliente = result.Read<mdlClientes_Cultivo_Listado>().ToList();
                mdl.info_contacto_cliente = result.Read<mdlClientes_Datos_Contacto>().ToList();
                mdl.validado = result.Read<mdl_Validado_Mercadotecnia_CRM>().FirstOrDefault();
                mdl.info_equip_cliente = result.Read<mdlClientes_EQUIP>().ToList();
                mdl.responsable_departamento = result.Read<mdl_Dep_Responsable_Seccion_CRM>().ToList();
                mdl.datos_persona_fisica = result.Read<mdlClientes_Datos_Persona_Fisica>().FirstOrDefault();

                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Opciones_Localidades_CRM>> Listado_localidades(string codigo_postal = null, int? idmunicipio = null)
        {
            try
            {
                var parametros = new
                {
                    codigo_postal = codigo_postal,
                    idmunicipio = idmunicipio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Opciones_Localidades_CRM> result = await factory.SQL.QueryAsync<mdl_Opciones_Localidades_CRM>("CRM.sp_Get_Localidades_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<int> GuardarClasificacion(mdl_Guarda_Clasificacion_Cliente_CRM mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    lineas = mdl.lineas,
                    giros = mdl.giros,
                    superficie = mdl.superficie,
                    nivel_tecnologia = mdl.nivel_tecnologia,
                    usuario = mdl.usuario
                };

                await factory.SQL.ExecuteAsync(
                    "CRM.sp_Guardar_Clasificacion_CRM",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return mdl.idcliente;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Info_Cliente_Asesores_CRM>> GuardaAsesorCliente(mdl_Guarda_Asesor_Cliente_CRM modelo)
        {
            try
            {
                var parametros = new
                {
                    idcliente = modelo.IdCliente,
                    idvendedor = modelo.IdVendedor,
                    idlinea = modelo.IdLinea,
                    usuario = modelo.Usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Info_Cliente_Asesores_CRM> result = await factory.SQL.QueryAsync<mdl_Info_Cliente_Asesores_CRM>(
                    "Credito.sp_rel_Cliente_Asesor_por_linea_Guardar",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Info_Cliente_Asesores_CRM>> CancelaAsesorCliente(int idcliente, int idvendedor, int idlinea, int usuario)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    idvendedor,
                    idlinea,
                    usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Info_Cliente_Asesores_CRM> result = await factory.SQL.QueryAsync<mdl_Info_Cliente_Asesores_CRM>("Credito.Cancela_Asesor_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlClientes_EQUIP>> GuardaEquipArray(int idcliente, string equip, int usuario)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    equip,
                    usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlClientes_EQUIP> result = await factory.SQL.QueryAsync<mdlClientes_EQUIP>("Credito.Cancela_Asesor_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Validado_Mercadotecnia_CRM> ValidaCliente(mdl_Guarda_Validacion_Cliente_CRM mdl)
        {
            try
            {
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    validado = mdl.validacion,
                    usuario = mdl.usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Validado_Mercadotecnia_CRM result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Validado_Mercadotecnia_CRM>("Credito.sp_Valida_Cliente_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
