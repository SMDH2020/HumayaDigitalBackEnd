using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.IndicadoresRecuperacion
{
    public class mdl_Indicadores_Semaforo_Recuperacion
    {
        public int idIndicador { get; set; }
        public string? tipo_indicador { get; set; }
        public string? tipo_cartera { get; set; }
        public string? semaforo { get; set; }
        public int ejercicio { get; set; }
        public double enero_minimo { get; set; }
        public double enero_maximo { get; set; }
        public double febrero_minimo { get; set; }
        public double febrero_maximo { get; set; }
        public double marzo_minimo { get; set; }
        public double marzo_maximo { get; set; }
        public double abril_minimo { get; set; }
        public double abril_maximo { get; set; }
        public double mayo_minimo { get; set; }
        public double mayo_maximo { get; set; }
        public double junio_minimo { get; set; }
        public double junio_maximo { get; set; }
        public double julio_minimo { get; set; }
        public double julio_maximo { get; set; }
        public double agosto_minimo { get; set; }
        public double agosto_maximo { get; set; }
        public double septiembre_minimo { get; set; }
        public double septiembre_maximo { get; set; }
        public double octubre_minimo { get; set; }
        public double octubre_maximo { get; set; }
        public double noviembre_minimo { get; set; }
        public double noviembre_maximo { get; set; }
        public double diciembre_minimo { get; set; }
        public double diciembre_maximo { get; set; }
        public bool autoriza_gerencia_finanzas { get; set; }
        public bool autoriza_gerencia_cobranza { get; set; }
        public string? usuario { get; set; }
    }
}
