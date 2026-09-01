namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Dashboard_View
    {
        public IEnumerable<mdl_Dashboard_Header>? header { get; set; }
        public IEnumerable<mdl_Dashboard_TotalCartera>? total { get; set; }
        public IEnumerable<mdl_Dashboard_TotalCartera_Estados>? total_estados { get; set; }
        public IEnumerable<mdl_Dashboard_Total_Cartera_Tabla>? tablatotal { get; set; }
        public IEnumerable<mdl_Dashboard_Total_Cartera_Tabla>? tabla { get; set; }
        public IEnumerable<mdl_Dashboard_RecuperacionCartera>? recuperacion { get; set; }
        public IEnumerable<mdl_Dashboard_GestionCobranza>? gestion { get; set; }
        public IEnumerable<mdl_Dashboard_PedidosFacturados>? pedidos { get; set; }
        public IEnumerable<mdl_Dashboard_ClientesJuridico>? juridico { get; set; }
        public IEnumerable<mdl_Dashboard_MensajesAutomaticos>? mensajes { get; set; }
        public IEnumerable<mdl_Dashboard_ProyeccionRecuperar_View>? proyeccion { get; set; }
        public IEnumerable<mdl_Dashboard_ProyeccionesRecuperar>? listado { get; set; }
        public string? tipo_cartera { get; set; }
        public string? columnas { get; set; }
        public mdl_Dashboard_Porcentaje_Recuperacion porc_recuperacion { get; set; }
        public IEnumerable<mdl_Permisos_Dash_Sucursales>? permisos { get; set; }
        public IEnumerable<mdl_Fecha_Ultima_Actualizacion>? ultima_actualizacion { get; set; }
        public mdl_cartera_transico? cartera_transito { get; set; }

    }
    public class mdl_cartera_transico
    {
        public double saldo { get; set; }
        public double registros { get; set; }
    }
}


