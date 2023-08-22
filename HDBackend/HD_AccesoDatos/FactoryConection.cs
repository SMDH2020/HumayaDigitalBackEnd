using System.Data;
using System.Data.SqlClient;

namespace HD.AccesoDatos
{
    public class FactoryConection 
    {
        private string _cadenaConexion = "";
        private SqlConnection connection;
        public FactoryConection(string cadenaconexion)
        {
            _cadenaConexion = cadenaconexion;
             connection = new SqlConnection(_cadenaConexion);
        }

        public void Close()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


        public IDbConnection SQL
        {
            get
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                return connection;
            }
        }
    }
}
