namespace HD_GestionActividades.Modelos
{
    public class mdl_Actividad
    {
        public int idactividad { get; set; }

        public int idgrupoactividades { get; set; }

        public string? nombreactividad { get; set; }

        public int sla { get; set; }

        public int tiemposolucion { get; set; }

        public string? tiempo { get; set; }

        public string? prioridad { get; set; }   // 'A' Alta / 'M' Mediana / 'B' Baja -- ya no la captura el usuario, la define este catálogo

        public bool estado { get; set; }

        public int usuario { get; set; }

        public string? nombregrupoactividades { get; set; }

    }
}