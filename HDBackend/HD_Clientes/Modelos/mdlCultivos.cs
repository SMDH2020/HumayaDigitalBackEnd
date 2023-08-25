namespace HD.Clientes.Modelos
{
    public class mdlCultivos
    {
        public int idcultivo { get; set; }

        public int idgiro_empresarial { get; set; }

        public string descripcion { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
