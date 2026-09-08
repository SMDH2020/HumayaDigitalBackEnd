namespace HD_GestionActividades.Modelos
{
    public class mdl_Sala
    {
        public int idsala { get; set; }

        public string? nombresala { get; set; }

        public string? tiposala { get; set; }

        public bool estado { get; set; }

        public int usuario { get; set; }

        // JSON crudo con la definición de campos extra que pide esta sala al
        // levantar un ticket (ej. [{"campo":"banco","etiqueta":"Banco",
        // "tipo":"lista","requerido":true,"opciones":["BBVA","Santander"]}]).
        // El backend no interpreta su contenido, solo lo guarda/regresa tal
        // cual -- el front arma y consume el JSON.
        public string? camposExtra { get; set; }

    }
}