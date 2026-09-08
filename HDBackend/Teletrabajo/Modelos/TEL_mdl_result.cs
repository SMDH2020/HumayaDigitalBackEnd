namespace Teletrabajo.Modelos
{
    public class TEL_mdl_result
    {
        public string? empleado { get; set; }
        public string? sucursal { get; set; }
        public string? puesto { get; set; }
        public string? foto { get; set; }
        public string? token { get; set; }
        public IEnumerable<string>? registros { get; set; }
    }
}
