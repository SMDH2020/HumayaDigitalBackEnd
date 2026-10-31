using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Reemplaza el conjunto completo de desarrolladores asignados a una
    // funcionalidad (una funcionalidad puede tener varios). UsuarioIds vacio
    // = quitar a todos los desarrolladores asignados.
    public class mdl_TI_FuncionalidadDesarrolladoresAsignar
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }

        public List<int> UsuarioIds { get; set; } = new();
    }
}
