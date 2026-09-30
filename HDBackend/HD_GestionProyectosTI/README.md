# HD_GestionProyectosTI

Módulo del proceso de solicitud, evaluación, priorización y seguimiento de
proyectos de TI (documento: *Proceso formal de solicitud, evaluación,
priorización y seguimiento de proyectos de TI*, v1.0). Cubre el alcance de
**Fase 1 (MVP)** del plan de implementación: captura, flujo básico de
estados, priorización, asignación de actividades y avance.

## Antes de compilar

1. Ejecutar en SQL Server, en ese orden:
   - `Database/01_CrearBaseYTablas.sql`
   - `Database/02_StoredProcedures_Fase1.sql`
   - `Database/03_Ajuste_AccesoInformacion.sql` (agrega el tipo "Acceso a
     información" y hace opcionales las 5 preguntas — ver v1.1 del documento)

   Deben correr en el **mismo servidor** que `humayadigital_usuarios`,
   porque el catálogo de módulos se reutiliza vía llamada en tres partes
   (`humayadigital_usuarios.dbo.sp_modulos_dropdownlist`), igual que ya
   hace `HD_Generales/AD_Modulos_Dropdownlist.cs`.

2. En `HD_Endpoints/appsettings.json` ya se agregó el connection string
   `GestionProyectosTI` apuntando a `192.168.0.51` con el mismo usuario que
   `Servicio`. Ajústalo si prefieres un login dedicado con permisos más
   acotados (recomendado a mediano plazo).

3. Dar de alta manualmente al primer Admin, corriendo:
   ```sql
   EXEC dbo.sp_RolTI_Asignar @idusuario = <idusuario_de_Guadalupe>, @rol = 'Admin', @asignadoPor = <mismo_id>;
   ```
   Sin esto nadie puede priorizar, asignar ni desglosar actividades — todos
   entran como 'Usuario' por default.

## Qué quedó implementado (Fase 1)

- `POST /api/SolicitudesTI/Crear` — captura con los 3 tipos, módulo y las 5 preguntas.
- `GET  /api/SolicitudesTI/Listado` — filtrada por rol (Usuario: solo suyas; Developer: suyas + asignadas; Admin: todas).
- `GET  /api/SolicitudesTI/Obtener/{id}` — detalle + actividades.
- `POST /api/SolicitudesTI/CambiarEstado` — Admin: revisar, aceptar, rechazar, cancelar, etc.
- `POST /api/SolicitudesTI/ResponderAprobacionAlcance` — el solicitante aprueba o rechaza el alcance documentado.
- `POST /api/SolicitudesTI/Priorizar` — Admin; detecta repriorización automáticamente y exige motivo.
- `POST /api/SolicitudesTI/CambiarFechaComprometida` — Admin, motivo obligatorio.
- `POST /api/ActividadesTI/Crear`, `GET Listado/{idsolicitud}`, `GET Mias`, `POST MarcarEstado`.
- `GET /api/RolTI/Mio`, `GET Listado`, `POST Asignar`.
- `GET /api/BitacoraTI/Historial` — bitácora completa (solo Admin por ahora).

## Qué falta (fases 2-5 del documento)

- Adjuntar evidencia/archivos a la solicitud (siguiendo el patrón de
  `HD_GestionActividades.SeguimientoAct.evidencia`).
- Notificación por correo en cambios de estado (reutilizar `HD_Notifications`,
  como hace `SeguimientoActController`).
- Calificación al cierre (idea tomada de `SeguimientoAct.calificacion`, no
  estaba en el documento original — vale la pena sumarla).
- Tablero con las 3 vistas (calendario de prioridades, incidencias por
  resolver, nuevas funcionalidades y mejoras) y reporte por módulo/periodo.
- Endpoint de bitácora "resumida" para que el Usuario vea su propio
  historial sin el detalle técnico completo.
- Frontend (React) que consuma estos endpoints.

## Notas de diseño

- El rol (`Usuario`/`Developer`/`Admin`) vive en `dbo.RolesTI`, tabla propia
  de este módulo — no toca el catálogo de roles general del sistema, que
  controla menús por módulo de negocio. No viaja en el JWT: cada endpoint
  lo resuelve en cada request a partir de `ISesion.usuario()`.
- La bitácora vive en `dbo.Bitacora` (no `Auditoria`, ese nombre ya lo usa
  el módulo de auditoría de inventario físico).
- Cuando el solicitante aprueba el alcance, la solicitud regresa al estado
  `Aceptada` (reutilizado como "lista para priorizar"); no se agregó un
  estado `Aprobada` aparte para no inflar el catálogo de estados.
