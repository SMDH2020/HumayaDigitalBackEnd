﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Buro.Modelos
{
    public class mdl_Buro_Credito
    {
        public int idburo { get; set; }
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public string? identificador_hd { get; set; }
        public Int16 institucion_hd { get; set; }
        public Int16 inst_ant_hd { get; set; }
        public Int16 tipo_institucion_hd { get; set; }
        public Int16 formato_hd { get; set; }
        public int fecha_hd { get; set; }
        public int periodo_hd { get; set; }
        public Int16 version_hd { get; set; }
        public string? usuario_hd { get; set; }
        public string? filler_hd { get; set; }
        public string? identificador_em { get; set; }
        public string? rfc_em { get; set; }
        public string? codigo_ciudadano_em { get; set; }
        public Int64 numero_dun_em { get; set; }
        public string? compañia_em { get; set; }
        public string? nombre1_em { get; set; }
        public string? nombre2_em { get; set; }
        public string? paterno_em { get; set; }
        public string? materno_em { get; set; }
        public string? nacionalidad_em { get; set; }
        public string? cal_banco_mex_em { get; set; }
        public Int64 banxico1_em { get; set; }
        public Int64 banxico2_em { get; set; }
        public Int64 banxico3_em { get; set; }
        public string? direccion1_em { get; set; }
        public string? direccion2_em { get; set; }
        public string? colonia_em { get; set; }
        public string? municipio_em { get; set; }
        public string? ciudad_em { get; set; }
        public string? estado_em { get; set; }
        public string? cp_em { get; set; }
        public string? telefono_em { get; set; }
        public string? extension_em { get; set; }
        public string? fax_em { get; set; }
        public Int16 tipo_cliente_em { get; set; }
        public string? estado_extranjero_em { get; set; }
        public string? pais_em { get; set; }
        public int clave_consolidacion_em { get; set; }
        public string? filler_em { get; set; }
        public string? identificador_ac { get; set; }
        public string? rfc_ac { get; set; }
        public string? codigo_ciudadano_ac { get; set; }
        public Int64 numero_dun_ac { get; set; }
        public string? nombre_cia_ac { get; set; }
        public string? nombre1_ac { get; set; }
        public string? nombre2_ac { get; set; }
        public string? paterno_ac { get; set; }
        public string? materno_ac { get; set; }
        public Int16 porcentaje_ac { get; set; }
        public string? direccion1_ac { get; set; }
        public string? direccion2_ac { get; set; }
        public string? colonia_ac { get; set; }
        public string? municipio_ac { get; set; }
        public string? ciudad_ac { get; set; }
        public string? estado_ac { get; set; }
        public string? cp_ac { get; set; }
        public string? telefono_ac { get; set; }
        public string? extension_ac { get; set; }
        public string? fax_ac { get; set; }
        public Int16 tipo_cliente_ac { get; set; }
        public string? estado_extranjero_ac { get; set; }
        public string? pais_ac { get; set; }
        public string? identificador_cr { get; set; }
        public string? rfc_empresa_cr { get; set; }
        public int numero_experiencias_cr { get; set; }
        public string? contrato_cr { get; set; }
        public string? contrato_anterior_cr { get; set; }
        public int fecha_apertura_cr { get; set; }
        public int plazo_meses_cr { get; set; }
        public Int16 tipo_credito_cr { get; set; }
        public Int64 saldo_inicial_cr { get; set; }
        public Int16 moneda_cr { get; set; }
        public Int16 numero_pagos_cr { get; set; }
        public int frecuencia_pagos_cr { get; set; }
        public Int64 importe_pagos_cr { get; set; }
        public int fecha_ultimo_pago_cr { get; set; }
        public int fecha_reestructura_cr { get; set; }
        public Int64 pago_efectivo_cr { get; set; }
        public Int64 fecha_liquidacion_cr { get; set; }
        public Int64 quita_cr { get; set; }
        public Int64 dacion_cr { get; set; }
        public Int64 quebranto_cr { get; set; }
        public string? observaciones_cr { get; set; }
        public string? especiales_cr { get; set; }
        public int fecha_primer_incum_cr { get; set; }
        public Int64 saldo_insoluto_cr { get; set; }
        public Int64 credito_max_utilizado_cr { get; set; }
        public int fecha_ingreso_cartera_vencida_cr { get; set; }
        public string? filler_cr { get; set; }
        public string? identificador_de { get; set; }
        public string? rdc_empresa_de { get; set; }
        public string? contrato_de { get; set; }
        public Int16 dias_vencido_de { get; set; }
        public Int64 cantidad_de { get; set; }
        public Int64 interes_de { get; set; }
        public string? filler_de { get; set; }
        public string? identificador_av { get; set; }
        public string? rfc_aval_av { get; set; }
        public string? codigo_ciudadano_av { get; set; }
        public Int64 numero_dun_av { get; set; }
        public string? nombre_cia_av { get; set; }
        public string? nombre1_av { get; set; }
        public string? nombre2_av { get; set; }
        public string? paterno_av { get; set; }
        public string? materno_av { get; set; }
        public string? direccion1_av { get; set; }
        public string? direccion2_av { get; set; }
        public string? colonia_av { get; set; }
        public string? municipio_av { get; set; }
        public string? ciudad_av { get; set; }
        public string? estado_av { get; set; }
        public string? cp_av { get; set; }
        public string? telefono_av { get; set; }
        public string? extension_av { get; set; }
        public string? fax_av { get; set; }
        public Int16 tipo_cliente_av { get; set; }
        public string? estado_extranjero_av { get; set; }
        public string? pais_av { get; set; }
        public string? filler_av { get; set; }
        public string? identificador_ts { get; set; }
        public int numero_compañias_ts { get; set; }
        public Int64 cantidad_ts { get; set; }
        public string? filler_ts { get; set; }

    }
}