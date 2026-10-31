using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Pedido
    {
        // Clave del pedido: unit_code + pedido
        [Required(ErrorMessage = "El unit_code es un valor requerido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El unit_code debe tener 2 caracteres")]
        public string? unit_code { get; set; }

        // Es numerico pero se guarda como texto para conservar ceros a la izquierda
        [Required(ErrorMessage = "El pedido es un valor requerido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El pedido debe tener 6 caracteres")]
        public string? pedido { get; set; }
        public string? codigo_pedido { get; set; }
        public string? enviado_desde { get; set; }
        public string? estado { get; set; }
        public string? modo_envio { get; set; }
        public string? cuenta_envio { get; set; }
        public string? enviar_a { get; set; }
        public string? numero_embarque { get; set; }
        public string? tipo_pedido { get; set; }
        public string? descripcion { get; set; }
        public string? pedido_cliente { get; set; }
        public string? grupo_enlazado { get; set; }
        public string? cotizacion { get; set; }
        public string? nombre_cliente { get; set; }
        public string? comentario_lista { get; set; }
        public string? distribuido_por { get; set; }
        public string? codigo_credito { get; set; }
        public string? numero_pedido { get; set; }

        // Llega como string ("11"); Newtonsoft lo convierte a entero
        public int? cantidad { get; set; }

        // Indicadores Y / N
        public string? intercambio_pendiente { get; set; }
        public string? retencion_transferencia { get; set; }
        public string? ind_no_cancelable { get; set; }

        // Por ahora llega null; se deja como texto hasta conocer su formato
        public string? precio_protegido { get; set; }

        // Fechas "YYYY-MM-DD" -> columnas DATE nullable
        public DateTime? entrega_fabrica { get; set; }
        public DateTime? entrega_original_fabrica { get; set; }
        public DateTime? fecha_estado { get; set; }
        public DateTime? fecha_factura { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public DateTime? fecha_origen { get; set; }
        public DateTime? fecha_atp { get; set; }
        public DateTime? fecha_ref_precio_protegido { get; set; }
        public DateTime? fecha_eta { get; set; }
        public DateTime? fecha_venta_renta { get; set; }
        public DateTime? fecha_venta_marcada { get; set; }
    }
}
