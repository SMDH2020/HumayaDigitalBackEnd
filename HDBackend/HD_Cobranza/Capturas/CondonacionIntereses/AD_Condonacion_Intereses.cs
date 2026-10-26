using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.CondonacionIntereses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.CondonacionIntereses
{
    public class AD_Condonacion_Intereses
    {
        private string CadenaConexion;
        public AD_Condonacion_Intereses(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Info_Condonacion_View> ObtenerLimiteUsuario(int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @usuario = usuario,
                };

                //var result = await

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.CondonacionInteres.sp_Obtener_informacion_Condonacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Info_Condonacion_View info = new mdl_Info_Condonacion_View();
                info.limites_usuario = result.Read<mdl_Limite_Condonacion_Usuario>().FirstOrDefault();
                info.reglas = result.Read<mdl_Reglas_Condonacion>().ToList();
                factory.SQL.Close();
                return info;
                //factory.SQL.QueryFirstOrDefaultAsync<mdl_Limite_Condonacion_Usuario>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Limite_Condonacion_Usuario",
                //parametros, commandType: System.Data.CommandType.StoredProcedure);
                //factory.SQL.Close();
                //return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Guarda_Condonacion_Interes>> Guardar(mdl_Guarda_Condonacion_Interes mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = mdl.idcliente,
                    @usuario = mdl.usuario,
                    @Saldo = mdl.Saldo,
                    @Inormal_saldo = mdl.Inormal_saldo,
                    @Inormal_porcentaje = mdl.Inormal_porcentaje,
                    @Inormal_descuento = mdl.Inormal_descuento,
                    @Inormal_pagado = mdl.Inormal_pagado,
                    @Imoratorio_saldo = mdl.Imoratorio_saldo,
                    @Imoratorio_porcentaje = mdl.Imoratorio_porcentaje,
                    @Imoratorio_descuento = mdl.Imoratorio_descuento,
                    @Imoratorio_pagado = mdl.Imoratorio_pagado,
                    @Comentarios = mdl.Comentarios,
                    @idreglaa_aplicada = mdl.idreglaa_aplicada,
                    @facturas = mdl.facturas,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Guarda_Condonacion_Interes>("Cartera_Clientes.CondonacionInteres.sp_Guardar_Condonacion_Interes_2",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Condonaciones_Cliente>> ObtenerCondonacionesCliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Listado_Condonaciones_Cliente>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonaciones_Cliente",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Validar_Condonaciones_Clientes> ValidarCondonacionesCliente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @idcliente = idcliente,
                };

                var result = await
                factory.SQL.QueryFirstOrDefaultAsync<mdl_Validar_Condonaciones_Clientes>("Cartera_Clientes.CondonacionInteres.sp_Validar_Condonaciones_Cliente",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Condonacion_Por_Folio_View> ObtenerCondonacionPorFolio(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @Folio = folio,
                };

                var result = await
                factory.SQL.QueryMultipleAsync("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonacion_Por_Folio",
                parametros, commandType: System.Data.CommandType.StoredProcedure);

                var view = new mdl_Condonacion_Por_Folio_View();
                view.condonacion = result.Read<mdl_Condonacion_Por_Folio>().FirstOrDefault();
                view.facturas = result.Read<mdl_Factura_Condonacion_Detalle>().ToList();

                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Autorizar_Condonacion>> Autorizar(mdl_Autorizar_Condonacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @Folio = mdl.Folio,
                    @usuario = mdl.usuario,
                    @aprobado = mdl.aprobado,
                    @Inormal_porcentaje = mdl.Inormal_porcentaje,
                    @Imoratorio_porcentaje = mdl.Imoratorio_porcentaje,
                    @ComentarioAutoriza = mdl.ComentarioAutoriza,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Autorizar_Condonacion>("Cartera_Clientes.CondonacionInteres.sp_Autorizar_Condonacion",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Condonacion_Reporte>> ObtenerCondonacionesReporte(int ejercicio, int periodo, string adr, string sucursal)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @ejercicio = ejercicio,
                    @periodo = periodo,
                    @adr = adr,
                    @sucursal = sucursal,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Condonacion_Reporte>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Condonaciones_Reporte",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
