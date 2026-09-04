namespace HD.Clientes.Modelos.CRM
{
    /// <summary>
    /// View del guardado de Datos de Buro de Credito: el SP regresa dos result sets,
    /// primero los datos de la persona fisica y despues el listado de domicilios.
    /// </summary>
    public class mdl_Datos_Buro_Credito_View
    {
        public mdlClientes_Datos_Persona_Fisica datos_persona_fisica { get; set; }
        public IEnumerable<mdlClientesDomicilioList> domicilios { get; set; }
    }
}
