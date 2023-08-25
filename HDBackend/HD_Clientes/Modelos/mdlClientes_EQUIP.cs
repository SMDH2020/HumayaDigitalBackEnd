namespace HD.Clientes.Modelos
{
    public class mdlClientes_EQUIP
    {
        public int idcliente_equip { get; set; }

        public int idcliente { get; set; }

        public string idequip { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
