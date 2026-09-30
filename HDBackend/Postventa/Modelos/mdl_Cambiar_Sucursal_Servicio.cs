using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Cambiar_Sucursal_Servicio
    {
        public int id_registro { get; set; }
        public string idsucursal { get; set; }
        public int usuario { get; set; }
    }
}
