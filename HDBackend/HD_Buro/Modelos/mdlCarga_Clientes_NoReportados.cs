﻿namespace HD_Buro.Modelos
{
    public class mdlCarga_Clientes_NoReportados
    {
        public string linea { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public string rfc { get; set; }
        public int totalfacturas { get; set; }
        public int vencidas { get; set; }
        public int porvencer { get; set; }
        public float saldo { get; set; }
        public bool registrado { get; set; }
        public bool tiene_domicilio { get; set; }
    }
}