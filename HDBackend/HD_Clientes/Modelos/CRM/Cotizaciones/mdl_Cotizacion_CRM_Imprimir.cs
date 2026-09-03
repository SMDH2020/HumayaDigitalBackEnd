using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizacion_CRM_Imprimir
    {
        public string folio_crm { get; set; }
        public string folio_equipo { get; set; }         // no disponible actualmente (queda vacío)
        public string asunto { get; set; }
        public string apreciable { get; set; }        // cot.nombre_contacto
        public string empresa { get; set; }             // Clientes.razon_social
        public string direccion { get; set; }            // no disponible actualmente (queda vacío)
        public string ciudad { get; set; }                // no disponible actualmente (queda vacío)
        public string sucursal { get; set; }              // Asesores.sucursal
        public string telefono_sucursal { get; set; }     // no disponible actualmente (queda vacío)
        public string sitio_web { get; set; }
        public string asesorventa { get; set; }           // Asesores.empleado
        public string atendio { get; set; }               // = asesorventa
        public string atentamente { get; set; }            // Asesores (filtrado por id_propietario).empleado
        public DateTime fecha { get; set; }                 // DateTime.Now (provisional)
        public string vigencia { get; set; }
        public string terminos { get; set; }                // texto genérico (provisional)
        public string moneda { get; set; }

        public double subtotal { get; set; }
        public double descuento_general { get; set; }        // cot.descuento
        public double ajuste { get; set; }
        public double total { get; set; }

        public string detalle { get; set; }                   // JSON serializado de List<mdl_DetalleCotizacionCRM>
    }

}