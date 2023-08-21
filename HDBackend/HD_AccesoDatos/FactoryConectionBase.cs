namespace HD.AccesoDatos
{
    public class FactoryConectionBase
    {
        public FactoryConection factory { get; private set; }
        public string Mensaje { get; set; } = "";
        public bool Valido { get; set; }
        public FactoryConectionBase(string CadenaConexion)
        {
            factory = new FactoryConection(CadenaConexion);
            Valido = true;
        }
    }
}