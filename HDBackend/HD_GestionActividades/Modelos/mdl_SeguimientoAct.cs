using System;

namespace HD_GestionActividades.Modelos
{
    public class mdl_SeguimientoAct
    {
        public int idSolicitud { get; set; }
        public int idSala { get; set; }

        public string? nombreSala { get; set; }          

        public int idActividad { get; set; }

        public string? nombreActividad { get; set; }    

        public string? comentarios { get; set; }
        public string? evidencia { get; set; }

        public DateTime? createDate { get; set; }
        public int? createUser { get; set; }

        public string? estatus { get; set; }

        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaTermino { get; set; }

        public int? usuarioAsignado { get; set; }
        public DateTime? fechaAsignacion { get; set; }

        public int usuario { get; set; }

        public string? usuarioNombre { get; set; }
        public string? fotoUsuario { get; set; }
        public int? idHistorial { get; set; }
        public string? tipoEvento { get; set; }
        public string? comentario { get; set; }
        public string? folio { get; set; }
        public string? prioridad { get; set; }

        // Solo se capturan cuando quien crea el ticket tiene rol de
        // administrador de soporte (ADTI); para el usuario normal quedan
        // en null y el ticket se guarda sin ellos.
        public int? idSucursal { get; set; }
        public int? idDepartamento { get; set; }

        // JSON crudo con las respuestas a los campos extra que pida la sala
        // elegida (ej. {"banco":"BBVA"}). El backend no lo interpreta, solo
        // lo guarda/regresa -- el front arma y consume el JSON según la
        // definición de Cat_Sala.camposExtra.
        public string? datosExtra { get; set; }

        public int? esResponsable { get; set; }
        public int? calificacion { get; set; }                 // 1 a 5 estrellas
        public string? comentarioCalificacion { get; set; }   // comentario si < 5
        public bool? calificado { get; set; }
    }
}