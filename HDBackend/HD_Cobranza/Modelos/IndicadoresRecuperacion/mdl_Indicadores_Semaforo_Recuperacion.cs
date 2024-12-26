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
        public int enero_minimo { get; set; }
        public int enero_maximo { get; set; }
        public int febrero_minimo { get; set; }
        public int febrero_maximo { get; set; }
        public int marzo_minimo { get; set; }
        public int marzo_maximo { get; set; }
        public int abril_minimo { get; set; }
        public int abril_maximo { get; set; }
        public int mayo_minimo { get; set; }
        public int mayo_maximo { get; set; }
        public int junio_minimo { get; set; }
        public int junio_maximo { get; set; }
        public int julio_minimo { get; set; }
        public int julio_maximo { get; set; }
        public int agosto_minimo { get; set; }
        public int agosto_maximo { get; set; }
        public int septiembre_minimo { get; set; }
        public int septiembre_maximo { get; set; }
        public int octubre_minimo { get; set; }
        public int octubre_maximo { get; set; }
        public int noviembre_minimo { get; set; }
        public int noviembre_maximo { get; set; }
        public int diciembre_minimo { get; set; }
        public int diciembre_maximo { get; set; }
        public bool autoriza_gerencia_finanzas { get; set; }
        public bool autoriza_gerencia_cobranza { get; set; }
        public string? usuario { get; set; }
    }
}
