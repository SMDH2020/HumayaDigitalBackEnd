using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopServices.VendedorMes
{
    public class GuardarVendedorMes
    {
        public static void Guardar(List<mdlVendedorMes> mdl)
        {
            FactoryConection factory = new FactoryConection();
            int posicion = 1;
            foreach (var item in mdl)
            {
                Console.WriteLine(factory.Mensaje);
                //Console.WriteLine("Nombre: {0}, Ventas: {1}", item.Nombre, item.Precio);
                var ultimoEspacio = item.nombre.LastIndexOf(" ");
                var nombre = item.nombre.Substring(0, ultimoEspacio);
                var apellido = item.nombre.Substring(ultimoEspacio + 1);

                var id = factory.SQL.QueryFirstOrDefault<int>("dashboard.sp_Obtener_ID_Vendedor_Por_Nombre", new { nombre = nombre, apellidopaterno = apellido },
                    commandType: CommandType.StoredProcedure);

                _ = factory.SQL.Query("dashboard.sp_vendedor_Mes_Guardar", new
                {
                    idvendedormes = -1,
                    ejercicio = item.año,
                    periodo = item.mes,
                    posicion = posicion++,
                    idempleado = id,
                    estatus = 1,
                    usuario = 0
                }, commandType: CommandType.StoredProcedure);


            }
        }
    }
}
