# HumayaDigitalBackEnd — Guía del proyecto

API REST en .NET para el ERP interno de Humaya Digital. Todo el código y los
comentarios se escriben en **español**.

---

## 1. Estructura de la solución

`HDBackend/HDBackend.sln` = **un proyecto Web API** + **N librerías de clases por
dominio de negocio**.

| Proyecto | Framework | Rol |
|---|---|---|
| `HD_Endpoints` | net8.0 | Web API. Controllers, middleware, `Program.cs`, `appsettings.json` |
| `HD_AccesoDatos` | net6.0 | `FactoryConection`, `Excepciones`, `mdlDropDownList` |
| `HD_Security` | net6.0 | JWT, `ISesion` / `Sesion` |
| `HD_Reporteria` | net6.0 | Generadores de Excel (ClosedXML) y PDF (QuestPDF) |
| `HD_Clientes`, `HD_Ventas`, `HD_Cobranza`, `HD_Auditoria`, … | net6.0 | Un proyecto por dominio |

Cada librería de dominio tiene la misma estructura interna:

```
HD_Dominio/
  Consultas/[Submodulo]/AD_Xxx.cs   ← acceso a datos (prefijo AD_)
  Modelos/[Submodulo]/mdlXxx.cs     ← POCOs (prefijo mdl)
  Reportes/                         ← generadores del dominio
```

No hay capa de servicios, ni repositorios con interfaz, ni DI para las clases AD:
**el controller instancia directo con `new`**. No introducir esas capas.

---

## 2. Reglas duras

Estas no se negocian. Si algo choca con ellas, preguntar antes de desviarse.

1. **Un archivo por clase y un archivo por tarea.** Nunca meter varios `mdl` en un
   mismo archivo, ni varios métodos de negocio distintos en un mismo `AD_`. Un AD
   por operación (`AD_Xxx_Guardar`, `AD_Xxx_Listado`, `AD_Xxx_GenerarSemanas`).
2. **Nombre del stored procedure vacío (`""`)** cuando el usuario no lo haya dado
   explícitamente. Él lo pega después. No inventarlo.
3. **El `usuario` nunca viaja en el request.** Se toma de `Sesion.usuario()` en el
   controller y se asigna al modelo o se pasa como parámetro. Cualquier valor que
   mande el cliente se ignora.
4. **Rutas siempre `[Route("/api/[controller]/[action]")]`.** Nada de kebab-case ni
   rutas literales, aunque una especificación externa las proponga.
5. **Dapper mapea por nombre exacto de columna.** Las propiedades se llaman igual
   que la columna del SP, en `snake_case`. Si no empata, la propiedad llega en su
   valor default **sin lanzar excepción** — es el bug más caro del proyecto.
6. **Columnas nullables en SQL → tipos nullables en C#** (`int?`, `DateTime?`,
   `decimal?`). Si no, Dapper truena en cuanto aparece un NULL.
7. **No reordenar en C# lo que el SP ya devolvió ordenado.** Usar `GroupBy` sin
   `OrderBy`: conserva el orden de aparición.
8. **Archivos `.cs` con BOM UTF-8**, como el resto del repo.
9. **Nunca escribir cadenas de conexión, contraseñas ni tokens** en respuestas,
   documentación o archivos nuevos.

---

## 3. Patrón del Controller

Heredan de `MyBase` (que ya trae `[Route("api/[controller]")] [ApiController]
[Authorize]`). Inyectan solo `IConfiguration` e `ISesion`.

```csharp
public class ClientesController : MyBase
{
    private readonly IConfiguration Configuracion;
    private readonly ISesion Sesion;
    public ClientesController(IConfiguration configuration, ISesion sesion)
    { Configuracion = configuration; Sesion = sesion; }

    [HttpGet]
    [Route("/api/[controller]/[action]")]
    public async Task<ActionResult> Listado(short filtrar)
    {
        string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        AD_Clientes_Listado datos = new AD_Clientes_Listado(CadenaConexion);
        var result = await datos.Listado(filtrar);
        return Ok(result);
    }
}
```

- La acción es de 4–6 líneas: cadena → `new AD_...` → `await` → `Ok(result)`.
- **Cero `try/catch` en el controller.** Lo maneja `ManejadorMiddlewares`.
- GET → parámetros sueltos en la firma. POST → un `mdl` completo como body.
- Los POST de guardado responden `Ok(new { mensaje = "Guardado Correctamente" })`
  o el ID. Nunca `CreatedAtAction`.
- Controllers agrupados por área: `Controllers/Credito/`, `Controllers/CRM/`,
  `Controllers/Ventas/`, …
- Se inyecta `ISesion` aunque la acción no lo use, por consistencia.

---

## 4. Patrón del AD (acceso a datos)

```csharp
public class AD_Clientes_Listado
{
    private string CadenaConexion;
    public AD_Clientes_Listado(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

    public async Task<IEnumerable<mdlClientes>> Listado(short filtrar)
    {
        try
        {
            var parametros = new { filtrar };
            FactoryConection factory = new FactoryConection(CadenaConexion);
            IEnumerable<mdlClientes> result = await factory.SQL.QueryAsync<mdlClientes>(
                "", parametros, commandType: System.Data.CommandType.StoredProcedure);
            factory.SQL.Close();
            return result;
        }
        catch (System.Exception ex)
        {
            throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
        }
    }
}
```

- **Siempre Dapper sobre stored procedures.** Cero SQL inline, cero EF.
- Objeto anónimo `parametros` con nombres idénticos a los del SP.
- `factory.SQL.Close()` antes de cada `return` (también en el `catch` cuando la
  operación escribe).
- Qué método de Dapper usar:
  - `QueryAsync<T>` → listados
  - `QueryFirstAsync<int>` → el SP devuelve el ID generado
  - `QueryMultipleAsync` → varios result sets (se arma un modelo `_View`)
  - `ExecuteAsync` → el SP no devuelve nada
- SPs nombrados `Esquema.sp_Modulo_Accion`. El esquema SQL corresponde al área
  (`Credito.`, `Ventas.`, `CRM.`).

### Guardados masivos

El detalle viaja al SP como **JSON en un parámetro `@json NVARCHAR(MAX)`** que se
desarma con `OPENJSON`. El front manda **objetos tipados**, y **el AD serializa**:

```csharp
var parametros = new
{
    ejercicio = mdl.ejercicio,
    json = JsonConvert.SerializeObject(mdl.detalle),
    usuario = mdl.usuario
};
```

Nunca recibir el detalle como `string` desde el front: pierde la validación de
modelo, la documentación de Swagger, y obliga a escapar JSON dentro de JSON.

El JSON va **plano** (una fila por celda), no anidado: se resuelve con un solo
`INSERT ... SELECT FROM OPENJSON`.

---

## 5. Patrón del Modelo

POCOs planos, propiedades en `snake_case` iguales a las columnas del SP.

```csharp
public class mdlClientes
{
    public int idcliente { get; set; }

    [Required(ErrorMessage = "El RFC es un valor requerido")]
    [StringLength(13, MinimumLength = 12, ErrorMessage = "...")]
    public string? rfc { get; set; } = "";

    public bool estatus { get; set; } = true;
    public string? usuario { get; set; } = "";
}
```

- Nullable habilitado, con valores por defecto (`= ""`, `= true`).
- DataAnnotations con mensajes en español **solo en los modelos de entrada**; los
  de salida son POCOs limpios.
- Validaciones útiles: `[Range]` para ids y rangos, `[MinLength(1)]` para listas,
  `IValidatableObject` para reglas cruzadas (ej. detectar ids repetidos).
- Para combos se reutiliza `mdlDropDownList` (`id` / `display`) de `HD_AccesoDatos`.
- Sufijo `_View` para el modelo que agrupa varios result sets de un
  `QueryMultipleAsync`.

---

## 6. Manejo de errores

`Excepciones` (en `HD_AccesoDatos`) lleva un `HttpStatusCode` y un objeto de error.
`ManejadorMiddlewares` lo serializa a JSON con el status correcto.

| Situación | Status | Cuerpo |
|---|---|---|
| Validación de modelo (DataAnnotations) | 400 | `{ "errors": { "campo": ["mensaje"] } }` |
| `THROW` del SP (validación de negocio) | 400 | `{ "Mensaje": "..." }` |
| Error no controlado | 500 | `{ "Mensaje": "..." }` |
| Token vencido | 401 | `{ "message": "Token Caducado" }` + header `Token-Expired` |

Los `THROW` definidos por el usuario en SQL Server obligan a un número ≥ 50000, así
que ese es el filtro correcto — **no listas ni rangos cerrados**:

```csharp
catch (SqlException ex) when (ex.Number >= 50000)
{
    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
}
catch (Exception ex)
{
    throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
}
```

Los mensajes del SP ya vienen redactados para el usuario final: **devolverlos tal
cual**, sin reescribir ni concatenar el stack.

---

## 7. Reportes Excel

Viven en `HD_Reporteria/<Area>/`, clase estática `XLS_Xxx` con
`GenerarExcel(...)`. Usan ClosedXML.

```csharp
int renglon = XLSEncabezado.Encabezado(ref sheet, "TITULO DEL REPORTE", totalColumnas);
// ... construir la tabla desde `renglon`
workbook.SaveAs(ruta);
byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
System.IO.File.Delete(ruta);
return Task.FromResult(new DocResult { documento = Convert.ToBase64String(docbytes), filename = filename });
```

- `XLSEncabezado.Encabezado` (en `HD_Ventas/Reportes/`) pinta el encabezado
  estándar (título gris + franja verde + franja ámbar) y devuelve el primer
  renglón libre. **Siempre usarlo**, nunca replicar el header del front.
- Ruta temporal: `C:\SMDH\Procesados\{filename}.xlsx`. El nombre debe distinguir
  las variantes del reporte (tipo, periodo) para que dos descargas no se pisen.
- `DocResult.filename` va **sin extensión**: el front agrega `.xlsx`.
- El controller expone una acción `ImprimirExcelXxx` que **reutiliza el mismo AD**
  del listado y le pasa el resultado al `XLS_`.
- Estilos compartidos de los indicadores del CRM (semáforo, filas banda, nombres de
  mes) están en `HD_Reporteria/CRM/XLS_IndicadoresEstilos.cs`. Si un color o
  formato se usa en más de un reporte, va ahí, no duplicado.

### Semáforo de cumplimiento (indicadores CRM)

| Condición | Relleno | Texto |
|---|---|---|
| `>= 100` | `#EAF3DE` | `#3B6D11` |
| `> 80 y < 100` | `#FDF3E3` | `#A76B0B` |
| `<= 80` | `#FDECEB` | `#C0392B` |
| sin objetivo (`null`) | sin relleno, texto `N/A` | `#9E9E9E` |

---

## 8. Receta para un endpoint nuevo

1. **SP en SQL Server** en el esquema del área: `Esquema.sp_Modulo_Accion`.
2. **Modelo(s)** en `HD_Dominio/Modelos/[Submodulo]/` — uno por archivo.
3. **Clase AD** en `HD_Dominio/Consultas/[Submodulo]/` — una por operación.
4. **Acción** en el controller del área correspondiente.
5. Si es dominio nuevo: crear la librería net6.0 con `Dapper` + referencia a
   `HD_AccesoDatos`, y agregar el `ProjectReference` en `HD_Endpoints.csproj`.

---

## 9. Trampas conocidas

- **`real` NO es palabra reservada en C#** (sí en SQL). Una propiedad puede
  llamarse `real` sin escaparla. `[Column]` no sirve: Dapper plano lo ignora, solo
  lo lee Dapper.Contrib.
- **`[Range(0, 100)]` sobre un `decimal`** usa el overload de `int` y puede fallar
  al convertir. Usar `[Range(typeof(decimal), "0", "9999999")]`.
- **`sheet.Columns().AdjustToContents()` rompe los encabezados combinados.** En
  tablas con celdas merge, fijar anchos a mano.
- **`FactoryConection` no implementa `IDisposable`**: el `Close()` manual se salta
  si algo truena antes de llegar a él.
- **Las librerías están en net6.0 y el API en net8.0.** Al usar sintaxis nueva de
  C# en una librería, verificar que compile en net6.0.
- **La ruta temporal de los Excel es fija por reporte**: dos usuarios exportando el
  mismo reporte al mismo tiempo se pisan el archivo.

---

## 10. Estilo de trabajo

- Antes de generar código nuevo, **leer un módulo equivalente** del repo y copiar
  su patrón, aunque una especificación externa proponga otra cosa. La consistencia
  con el repo gana sobre la especificación.
- Cuando una especificación externa contradiga estas convenciones (rutas,
  nombres, estructura), **seguir el repo y avisarlo**, no aplicarla en silencio.
- Cuando falte información para decidir (formato de un campo, si una columna la
  devuelve el SP), **plantear la duda antes de generar**, no adivinar.
- Marcar explícitamente cualquier suposición que se haya hecho al entregar.
