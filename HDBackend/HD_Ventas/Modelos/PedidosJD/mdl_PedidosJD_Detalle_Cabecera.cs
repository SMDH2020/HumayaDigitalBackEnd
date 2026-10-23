using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    /// <summary>
    /// Cabecera del detalle de un pedido. Los nombres de las propiedades son exactamente los
    /// del JSON de John Deere (y de las columnas de Ventas.jd_pedidos_detalle), por eso no van en snake_case.
    /// </summary>
    public class mdl_PedidosJD_Detalle_Cabecera
    {
        [Required(ErrorMessage = "El unit_code es un valor requerido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El unit_code debe tener 2 caracteres")]
        public string? unit_code { get; set; }

        [Required(ErrorMessage = "El pedido es un valor requerido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El pedido debe tener 6 caracteres")]
        public string? pedido { get; set; }

        // Fechas "YYYY-MM-DD"
        public DateTime? orderDate { get; set; }
        public DateTime? customerOrderDate { get; set; }
        public DateTime? priceProtectedDate { get; set; }
        public DateTime? transferHoldEndsDate { get; set; }
        public DateTime? availabelDate { get; set; }
        public DateTime? factoryDeliveryDate { get; set; }
        public DateTime? origFactDeliveryDate { get; set; }
        public DateTime? requestedDeliveryDate { get; set; }
        public DateTime? requestedShipDate { get; set; }
        public DateTime? shippedDate { get; set; }
        public DateTime? invoicedDate { get; set; }
        public DateTime? expectedWHSDate { get; set; }
        public DateTime? productionStartDate { get; set; }
        public DateTime? priceProtectRefDate { get; set; }
        public DateTime? sourceDate { get; set; }
        public DateTime? markedRetailDate { get; set; }
        public DateTime? willCallDate { get; set; }
        public DateTime? sailDate { get; set; }
        public DateTime? etaDATE { get; set; }
        public DateTime? atpDate { get; set; }
        public DateTime? moveUpDate { get; set; }

        // Texto (weight tambien va como texto, sin convertir)
        public string? codigo_pedido { get; set; }
        public string? descripcion_pedido { get; set; }
        public string? customerOrderNo { get; set; }
        public string? earlyShipOrder { get; set; }
        public string? typeOrder { get; set; }
        public string? emergencyOrder { get; set; }
        public string? orderStatus { get; set; }
        public string? cancellableOrder { get; set; }
        public string? transferHold { get; set; }
        public string? releasedFrom { get; set; }
        public string? onHoldOrder { get; set; }
        public string? orderKind { get; set; }
        public string? shippingFrom { get; set; }
        public string? creditCode { get; set; }
        public string? weight { get; set; }
        public string? salesProgram { get; set; }
        public string? machineStatus { get; set; }
        public string? priceProtectType { get; set; }
        public string? soldToAcctCityState { get; set; }
        public string? shippingMode { get; set; }
        public string? loadNumber { get; set; }
        public string? consolidatedLoad { get; set; }
        public string? carrierName { get; set; }
        public string? carrierNotified { get; set; }
        public string? vehicleNumber { get; set; }
        public string? containerNumber { get; set; }
        public string? shipToName { get; set; }
        public string? shipToAddress1 { get; set; }
        public string? shipToAddress2 { get; set; }
        public string? shipToCityStateZip { get; set; }
        public string? deliveryCityStateZip { get; set; }
        public string? spclSHPGINST1 { get; set; }
        public string? spclSHPGINST2 { get; set; }
        public string? oceanCarrier { get; set; }
        public string? vesselName { get; set; }
        public string? commercialInvoice { get; set; }
        public string? packagingType { get; set; }
        public string? versionNumber { get; set; }
        public string? linkedGroup { get; set; }
        public string? quoteNumber { get; set; }
    }
}
