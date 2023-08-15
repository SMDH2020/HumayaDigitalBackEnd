namespace HD_AccesoDatos
{
    public class FactoryConectionBase
    {
        public FactoryConnection factory;
        public string Mensaje { get; set; }
        public FactoryConectionBase(string CadenaConexion)
        {
            factory = new FactoryConnection(CadenaConexion);
        }
    }
}
