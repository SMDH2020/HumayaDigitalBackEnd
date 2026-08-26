namespace HD.Clientes.Modelos
{
    public class mdlClientes_EQUIP
    {
        public int idcliente_equip { get; set; }
        public int idcliente { get; set; }
        public string? idequip { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public bool estatus { get; set; }
        public string? usuario { get; set; } = "";
        public int? id_ultimousuario { get; set; }
        public string? ultimousuario { get; set; }
        public DateTime? ultima_fecha { get; set; }
        public string? accion { get; set; }
    }
}
