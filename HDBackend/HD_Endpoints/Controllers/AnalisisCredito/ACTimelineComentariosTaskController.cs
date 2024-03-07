﻿using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito
{
    public class ACTimelineComentariosTaskController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACTimelineComentariosTaskController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSCAnalisisComentariosTask mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios_Task datos = new ADAnalisis_Comentarios_Task(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
