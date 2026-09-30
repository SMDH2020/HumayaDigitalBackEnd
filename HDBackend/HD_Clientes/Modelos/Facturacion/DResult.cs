namespace HD.Clientes.Modelos.Facturacion
{
    public class DResult
    {
        public bool valido { get; set; }
        public string mensaje { get; set; }
        public object result { get; set; }
        public DResult()
        {
            valido = true;
            mensaje = "Datos Registrados Con Exito";
            result = null;
        }
    }
}
