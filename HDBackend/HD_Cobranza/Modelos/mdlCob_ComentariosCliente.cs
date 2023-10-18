﻿namespace HD_Cobranza.Modelos
{
    public class mdlCob_ComentariosCliente
    {
        public long idcomentario { get; set; }
        public int idcliente { get; set; }
        public string? comentarios { get; set; }
        public string? formacontacto { get; set; }
        public bool compromisopago { get; set; }
        public DateTime? fechacompromisopago { get; set; }
        public double importeconvenio { get; set; }
        public bool recordatorio { get; set; }
        public DateTime? fecharecordatorio { get; set; }
        public string? usuario { get; set; }
    }
}
