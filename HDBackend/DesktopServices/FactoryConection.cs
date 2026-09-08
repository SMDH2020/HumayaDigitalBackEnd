using System.Data;
using System.Data.SqlClient;

namespace DesktopServices
{
    public class FactoryConection
    {
        private SqlConnection connection;
        private string _CadenaConexion;
        public string Mensaje { get; private set; }
        public FactoryConection(string cadenaconexion="Data Source=192.168.0.51; Initial Catalog=HumayaDigital; user=sa; password=Mhu820315ut3;", bool _opentransaccion = false)
        {
            _CadenaConexion = cadenaconexion;
            //_CadenaConexion = "Data Source=DESKTOP-Q0V3OC1\\DEVELOPER; Initial Catalog=equip; integrated security=true;";

            if (_opentransaccion)
                OpenTransaccion();
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
                    connection = new SqlConnection(_CadenaConexion);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                return connection;
            }
        }

        public IDbTransaction transaccion { get; private set; }
        private bool OpenTransaccion()
        {
            try
            {
                if (connection is null)
                    connection = new SqlConnection(_CadenaConexion);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                transaccion = connection.BeginTransaction();
                return true;
            }
            catch (System.Exception ex)
            {
                Mensaje = ex.Message;
                return false;
            }
        }
    }
}
