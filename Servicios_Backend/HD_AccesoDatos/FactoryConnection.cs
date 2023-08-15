using System.Data;
using System.Data.SqlClient;

namespace HD_AccesoDatos
{
    public class FactoryConnection
    {
        private string _cadenaConexion;
        private SqlConnection connection;
        public FactoryConnection(string cadenaconexion)
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
