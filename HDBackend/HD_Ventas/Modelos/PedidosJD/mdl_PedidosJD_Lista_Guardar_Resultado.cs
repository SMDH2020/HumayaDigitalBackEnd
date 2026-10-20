namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Lista_Guardar_Resultado
    {
        public int recibidos { get; set; }
        public int nuevos { get; set; }
        public int actualizados { get; set; }
        public List<mdl_PedidosJD_Pendiente> pendientes_detalle { get; set; } = new List<mdl_PedidosJD_Pendiente>();
    }
}
