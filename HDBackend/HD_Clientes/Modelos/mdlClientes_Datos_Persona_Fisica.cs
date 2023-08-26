namespace HD.Clientes.Modelos
{
    public class mdlClientes_Datos_Persona_Fisica : mdlClientes
    {

        public string nombre { get; set; }

        public string apellido_paterno { get; set; }

        public string apellido_materno { get; set; }

        public string curp { get; set; }

        public string sexo { get; set; }

        public string estado_civil { get; set; }
        public int edad { get; set; }
        public string regimen_conyugal { get; set; } = "NA";

    }
}
