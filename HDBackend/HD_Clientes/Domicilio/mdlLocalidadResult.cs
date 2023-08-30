using HD.AccesoDatos;

namespace HD.Clientes.Domicilio
{
    public class mdlLocalidadResult
    {
        public mdlEstadoMunicipio? estado { get; set; } = new mdlEstadoMunicipio();
        public IEnumerable<mdlDropDownList> localidades { get; set; }= new List<mdlDropDownList>();
    }
}
