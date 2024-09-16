using Dapper;
using HD_Finanzas.Modelos.Balance_General;
using Modelo.Utilities;
using HD_Finanzas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HD.AccesoDatos;

namespace Enlace.Dapper.Reportes
{
    public class BalacenGeneralResult
    {
        public IEnumerable<Fmdl_BalanceGeneral> balance{ get; set; }
        public vmCargaBalanza infobalanza { get; set; }

    }
    public class AD_BalanceGeneral
    {
        private string CadenaConexion;
        public AD_BalanceGeneral(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<BalacenGeneralResult> GetBalanceGeneral(vmBalanceGeneral vm)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var prm = new
                {
                    ejercicio = vm.Ejercicio,
                    periodo = vm.periodo,
                    adr=vm.adr,
                    sucursales=vm.sucursales
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.SP_Obtener_BalanceGeneralOtro", prm, commandType: System.Data.CommandType.StoredProcedure);
                IEnumerable<Fmdl_BalanceGeneral> balance = result.Read<Fmdl_BalanceGeneral>().ToList();
                vmCargaBalanza infobalanza = result.Read<vmCargaBalanza>().FirstOrDefault();
                factory.SQL.Close();
                return new BalacenGeneralResult()
                {
                    balance = balance,
                    infobalanza=infobalanza
                };
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        async Task<List<BalanceConsolidado>> BalanceConsolidado(vmBalanceGeneral vm,int index,List<BalanceConsolidado> BCG)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var prm = new
                {
                    ejercicio = vm.Ejercicio,
                    periodo = index,
                    adr = vm.adr,
                    sucursales = vm.sucursales
                };
                var result = await factory.SQL.QueryAsync<Fmdl_BalanceGeneral>("PixelCode.dbo.SP_Obtener_BalanceGeneralConsolidado", prm, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                BCG.Add(new HD_Finanzas.Modelos.Balance_General.BalanceConsolidado
                {
                    Periodo = index,
                    BalanceGeneral = result
                });
                if(index>= vm.periodo)
                {
                    return BCG;
                }
                else
                {
                    return await BalanceConsolidado(vm, index + 1, BCG);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<BalanceConsolidado>> GetBalanceConsolidado(vmBalanceGeneral vm)
        {
            try
            {
                List<BalanceConsolidado> BCG = new List<BalanceConsolidado>();
                var  result= await BalanceConsolidado(vm, 1, BCG);
                if(result.Count == 0)
                {
                    throw new Exception("No existe información para mostrar");
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message); 
            }
        }

    }
}
