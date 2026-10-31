using System;

namespace HD_GestionActividades.Modelos
{
    public class mdl_SalaActividad
    {
        public int idRelSalaActividad { get; set; }
        public int idSala { get; set; }
        public int idActividad { get; set; }

        public DateTime? createDate { get; set; }
        public short? createUser { get; set; }

        public DateTime? updateDate { get; set; }
        public short? updateUser { get; set; }

        public bool estado { get; set; }

        public int usuario { get; set; }
    }
}