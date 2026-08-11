namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_CRM_ID_View
    {
        public mdl_Info_Cliente_CRM info_General_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_estatus { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_origen { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_tipo { get; set; }
        public IEnumerable<mdl_Info_Cliente_Ubicacion_CRM> info_ubicacion_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Estado_CRM> opciones_estado { get; set; }
        public IEnumerable<mdl_Opciones_Municipio_CRM> opciones_municipio { get; set; }


    }
}
