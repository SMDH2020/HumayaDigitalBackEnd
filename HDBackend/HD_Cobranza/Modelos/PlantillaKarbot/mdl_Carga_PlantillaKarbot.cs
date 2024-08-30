namespace HD_Cobranza.Modelos.PlantillaKarbot
{
    public class mdl_Carga_PlantillaKarbot
    {
        public int idcliente {  get; set; }
        public string? Cliente { get; set; }
        public string? contacto { get; set; }
        public float saldo { get; set; }
        public string? ReferenciaBancaria { get; set; }
        public int aprobado { get; set; }
        public int enviado { get; set; }

    }
}
