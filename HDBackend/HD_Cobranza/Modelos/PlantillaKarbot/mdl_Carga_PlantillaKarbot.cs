namespace HD_Cobranza.Modelos.PlantillaKarbot
{
    public class mdl_Carga_PlantillaKarbot
    {
        public int idcliente {  get; set; }
        public string? Cliente { get; set; }
        public string? contacto { get; set; }
        public float saldo { get; set; }
        public string? referencia { get; set; }
        public int aprobado { get; set; }
        public int enviado { get; set; }

    }
    public class mdl_Carga_PlantillaKarbotOperacion
    {
        public int idcliente { get; set; }
        public string? Cliente { get; set; }
        public string? contacto { get; set; }
        public float saldo { get; set; }
        public float capital { get; set; }
        public float interes_pactado { get; set; }
        public float interes_moratorio { get; set; }
        public string? referencia { get; set; }
        public int aprobado { get; set; }
        public int enviado { get; set; }

    }
}
