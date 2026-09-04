namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_CRM_ID_View
    {
        public mdl_Info_Cliente_CRM info_General_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_estatus { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_origen { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_tipo { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_clasificacion { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_superficie { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_tecnologia { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_etiqueta { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_cultivo_terreno { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> opciones_cultivo_riego { get; set; }
        public IEnumerable<mdl_Info_Cliente_Ubicacion_CRM> info_ubicacion_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Estado_CRM> opciones_estado { get; set; }
        public IEnumerable<mdl_Opciones_Municipio_CRM> opciones_municipio { get; set; }
        public mdl_Info_Cliente_Facturacion_CRM info_Facturacion_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Lineas_CRM> opciones_lineas { get; set; }
        public IEnumerable<mdl_Opciones_Giros_CRM> opciones_giros { get; set; }
        public mdl_Info_Cliente_Clasificación_CRM info_clasificacion_cliente { get; set; }
        public IEnumerable<mdl_Opciones_Asesor> opciones_asesor { get; set; }
        public IEnumerable<mdl_Info_Cliente_Asesores_CRM> info_asesores_cliente { get; set; }
        public IEnumerable<mdlClientes_Cultivo_Listado> info_cultivo_cliente { get; set; }
        public IEnumerable<mdlClientes_Datos_Contacto> info_contacto_cliente { get; set; }
        public mdl_Validado_Mercadotecnia_CRM validado { get; set; }
        public IEnumerable<mdlClientes_EQUIP> info_equip_cliente { get; set; }
        public IEnumerable<mdl_Dep_Responsable_Seccion_CRM> responsable_departamento { get; set; }
        public mdlClientes_Datos_Persona_Fisica datos_persona_fisica { get; set; }

    }
}
