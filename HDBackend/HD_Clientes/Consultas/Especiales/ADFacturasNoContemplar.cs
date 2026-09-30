using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Especiales;
using System.Security.Cryptography;

namespace HD.Clientes.Consultas.Especiales
{
    public class ADFacturasNoContemplar
    {
        string CadenaConexion = "";
        public ADFacturasNoContemplar(string _CadenaConexion)
        {
            CadenaConexion = _CadenaConexion;
        }
        public async Task<bool> Guardar(mdlFacturasnocontemplar mdl)
        {
            try
            {
                var parametros = new
                {
                    documento = mdl.documento,
                    comentarios = mdl.comentarios,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Facturas_no_contemplar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlFacturasnocontemplar_List>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlFacturasnocontemplar_List>("Credito.sp_Facturas_no_contemplear_Lista", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlFacturasnocontemplar_List>> Eliminar(string documento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    documento
                };
                var result = await factory.SQL.QueryAsync<mdlFacturasnocontemplar_List>("Credito.sp_Facturas_no_contemplar_eliminar",parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlFacturasNoContemplar_Buscar>> Buscar(int documento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    documento = documento,
                };
                var result = await factory.SQL.QueryAsync<mdlFacturasNoContemplar_Buscar>("Credito.sp_Buscar_Factura_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlFacturasnocontemplar_List>> InfoFactura(int documento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    documento = documento,
                };
                var result = await factory.SQL.QueryAsync<mdlFacturasnocontemplar_List>("Credito.sp_Obtener_Info_Factura", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
