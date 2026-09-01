using System.ComponentModel.DataAnnotations;

namespace Postventa.Modelos.OrdenesAbiertas
{
    public class mdl_OT_Comentario_Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class mdl_OT_Comentario_Item
    {
        public int Id { get; set; }
        public int OrdenTrabajoId { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public string Comentario { get; set; }
        public System.DateTime? FechaEstimadaCierre { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
    }

    public class mdl_OT_Comentario_Guardar
    {
        [Required(ErrorMessage = "La orden de trabajo es requerida")]
        public int OrdenTrabajoId { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El comentario es requerido")]
        public string Comentario { get; set; }

        public System.DateTime? FechaEstimadaCierre { get; set; }

        public string? UsuarioRegistro { get; set; }
    }
}
