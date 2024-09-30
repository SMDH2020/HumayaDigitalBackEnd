

namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Gestiones_Realizadas_Comentario
    {
        public int idgestion { get; set; }
        public int IDEstado  { get; set; }
        public int IDSucursal { get; set; }
        public int idcliente { get; set; }
        public string? RazonSocial { get; set; }
        public string? comentarios { get; set; }
        public int idresponsable { get; set; }
        public string? responsable { get; set; }
        public string? volvercontactar { get; set; }
        public DateTime? fechavolvercontactar { get; set; }
        public float saldo { get; set; }
        public float moratorios { get; set; }
        public float interespactado { get; set; }
        public float total { get; set; }
        public DateTime createdate { get; set; }
        public int createuser { get; set; }

    }
}
