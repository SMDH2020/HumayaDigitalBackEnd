using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
   

    public class mdl_EstimacionVentas_ADR
    {
        public int idadr { get; set; }
        public string adr { get; set; }
    }

    public class mdl_EstimacionVentas_Sucursal
    {
        public int idsucursal { get; set; }
        public string descripcion { get; set; }
    }

    public class mdl_EstimacionVentas_Captura
    {
        public int captura { get; set; }
    }

    public class mdl_EstimacionVentas_Resumen
    {
        public string linea { get; set; }
        public decimal ventaestimada { get; set; }
        public decimal objetivo { get; set; }
    }

    
    public class mdl_VentasEstimadas_Resultado
    {
        public List<mdl_EstimacionVentas_ADR> adr { get; set; }
        public List<mdl_EstimacionVentas_Sucursal> sucursales { get; set; }
        public mdl_EstimacionVentas_Captura captura { get; set; }
        public List<mdl_EstimacionVentas_Resumen> resumen { get; set; }
    }


    public class mdl_EstimacionVentasDetalle
    {
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public int idsucursal { get; set; }
        public int idlinea { get; set; }
        public string linea { get; set; }
        public decimal ventas { get; set; }
        public decimal objetivo { get; set; }
        public decimal vta_origen { get; set; }
    }


    public class mdl_GuardarEstimacionVentas
    {
        [Required(ErrorMessage = "El detalle es un valor requerido")]
        public string detalle { get; set; }
        public int usuario { get; set; }
    }
}