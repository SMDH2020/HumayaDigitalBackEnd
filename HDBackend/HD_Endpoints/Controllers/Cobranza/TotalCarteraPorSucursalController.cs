﻿using HD.Clientes.Consultas.ClientesDomicilio;
using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class TotalCarteraPorSucursalController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TotalCarteraPorSucursalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorSucursal datos = new ADCob_TotalCarteraPorSucursal(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorSucursal datos = new ADCob_TotalCarteraPorSucursal(CadenaConexion);
            var result = await datos.Listado();
            var docresult = await XLSCob_TotalCartera_Sucursal.CrearResumenPorSucursal(result);
            return Ok(docresult);

        }
    }
}