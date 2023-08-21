using System.Data;
using Microsoft.Data.SqlClient;

namespace HD.AccesoDatos
{
    public class FactoryConection
    {
        private string _cadenaConexion="";
        private SqlConnection connection= new SqlConnection();
        public FactoryConection(string cadenaconexion)
        {
              _cadenaConexion = cadenaconexion;
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
                if (connection is null)
                    connection = new SqlConnection(_cadenaConexion);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                return connection;
            }
        }
    }
}