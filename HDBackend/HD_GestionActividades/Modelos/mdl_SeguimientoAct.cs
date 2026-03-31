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
        public int? esResponsable { get; set; }
    }
}