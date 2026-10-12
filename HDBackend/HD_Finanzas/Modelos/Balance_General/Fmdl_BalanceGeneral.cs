namespace HD_Finanzas.Modelos.Balance_General
{
    public class Fmdl_BalanceGeneral
    {
        public string? nivel1 { get; set; }
        public string? nivel2 { get; set; }
        public string? nivel3 { get; set; }
        public string? nivel4 { get; set; }
        public double total { get; set; }
        public double mrgtotal { get; set; }
        public double totalanterior { get; set; }
        public double mrgtotalanterior { get; set; }
        public double variacion { get; set; }
        public double mrgvariacion { get; set; }
    }

    public class BalanceConsolidado
    {
        public int Periodo { get; set; }
        public IEnumerable<Fmdl_BalanceGeneral> BalanceGeneral { get; set; }
    }
}
