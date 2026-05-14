namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Listado_Inventario_Conteo_Piezas
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string familia { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public float existencia_orig { get; set; }
        public float cant_surtido { get; set; }
        public float cant_transito { get; set; }
        public string? unidad_medida { get; set; }
        public float costo_unitario { get; set; }
        public string posicion { get; set; }
        public bool ubicacion_ok { get; set; }
        public float diferencia { get; set; }
        public string tipo_diferencia { get; set; }
        public float conteo_fisico { get; set; }
        public float justificadas { get; set; }

    }
}
