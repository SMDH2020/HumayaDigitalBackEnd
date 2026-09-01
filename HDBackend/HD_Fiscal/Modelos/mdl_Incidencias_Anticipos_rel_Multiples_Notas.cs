namespace HD.Fiscal.Modelos
{
    public class mdl_Incidencias_Anticipos_rel_Multiples_Notas
    {
        public string v_ref { get; set; }
        public float total_abonos { get; set; }
        public float total_cargos { get; set; }
        public float saldo { get; set; }
        public string movimiento { get; set; }
        public string fecha_mov { get; set; }
        public string batch_mov { get; set; }
        public float cargo_mov { get; set; }
        public float abono_mov { get; set; }
        public string descripcion { get; set; }
        public string usuario { get; set; }
        public string relacionado { get; set; }
        public string v_fecha_relacion { get; set; }

    }
}
