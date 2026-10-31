using HD.Security;
using HD_GestionProyectosTI.Consultas.Backlog;
using HD_GestionProyectosTI.Consultas.Roles;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    // Backlog mensual: revision del desarrollo del mes entre Jefatura de TI
    // y Planeacion Estrategica (rol GERENCIA_PLANEACION). Ver consultas y
    // escritura (programar/reprogramar una funcionalidad existente) la hace
    // PriorizacionTIController -- este controller solo agrega lo que falta
    // para esta pantalla: los dos listados (mensual y buscar) y las dos
    // acciones nuevas (agregar funcionalidad extra, actualizar avance).
    public class BacklogTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public BacklogTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        private static readonly string[] RolesVerBacklog = { "JEFATURA_TI", "GERENCIA_PLANEACION", "ADMIN_MODULO" };

        private async Task<bool> PuedeVerBacklogAsync(int idusuario)
        {
            var roles = await new HD_GestionProyectosTI.Consultas.Roles.AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => RolesVerBacklog.Contains(r.Codigo));
        }

        // Jefatura de TI es quien programa, agrega extras y actualiza el
        // avance; Planeacion Estrategica solo tiene acceso de lectura en
        // esta pantalla (igual que ya documenta 09_ProgramacionMensual.sql
        // para el resto del modulo de priorizacion).
        private async Task<bool> EsJefaturaTIAsync(int idusuario)
        {
            var roles = await new HD_GestionProyectosTI.Consultas.Roles.AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => r.Codigo == "JEFATURA_TI");
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Mensual(byte mes, short anio)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await PuedeVerBacklogAsync(idusuario))
                return Forbid();

            var result = await new AD_TI_Backlog_MensualListado(CadenaConexion).Listado(mes, anio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Buscar(byte mes, short anio, string? busqueda = null)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await PuedeVerBacklogAsync(idusuario))
                return Forbid();

            var result = await new AD_TI_Backlog_Buscar(CadenaConexion).Buscar(mes, anio, busqueda);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarExtra(mdl_TI_FuncionalidadExtraCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            var (funcionalidadId, solicitudId) = await new AD_TI_FuncionalidadesExtra_Agregar(CadenaConexion).Agregar(mdl, idusuario);
            return Ok(new { funcionalidadId, solicitudId, mensaje = "Funcionalidad agregada al backlog correctamente" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarAvance(mdl_TI_FuncionalidadAvanceActualizar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_ActualizarAvance(CadenaConexion).ActualizarAvance(mdl, idusuario);
            return Ok(new { mensaje = "Avance actualizado" });
        }

        // Catalogo de usuarios con rol TI "DESARROLLADOR" (ver
        // 27_BacklogDesarrolladoresYEliminarFuncionalidad.sql) -- lectura
        // disponible para quien pueda ver el backlog, igual que el resto de
        // los listados de esta pantalla.
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DesarrolladoresDisponibles()
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await PuedeVerBacklogAsync(idusuario))
                return Forbid();

            var result = await new AD_TI_Roles_UsuariosPorRol(CadenaConexion).Listado("DESARROLLADOR");
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AsignarDesarrolladores(mdl_TI_FuncionalidadDesarrolladoresAsignar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_AsignarDesarrolladores(CadenaConexion).Asignar(mdl, idusuario);
            return Ok(new { mensaje = "Desarrolladores asignados" });
        }

        // Elimina la funcionalidad por completo (no solo la quita del
        // backlog del mes, como ya hace PriorizacionTI/RegresarABacklog) --
        // baja logica (Activa = 0), desaparece de todos los listados activos
        // del modulo.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarFuncionalidad(mdl_TI_FuncionalidadEliminar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_Eliminar(CadenaConexion).Eliminar(mdl, idusuario);
            return Ok(new { mensaje = "Funcionalidad eliminada" });
        }
    }
}
