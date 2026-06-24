using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class ConexionBD
    {
        //Cadena que conecta la base de datos con el codigo
        // string cadenaConexion = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Fulbito500;Integrated Security=True;";
        // cadena para máquina de Lucas, descomentar si es necesario
        string cadenaConexion = @"Data Source=localhost\SQLEXPRESS01;Initial Catalog=Fulbito500;Integrated Security=True;";

        //Metodo que valida la conexion con la base de datos
        public SqlConnection ValidarConexion()
        {
            try
            {
                SqlConnection conexion = new SqlConnection(cadenaConexion);
                return conexion;
            }
            catch
            {
                throw new Exception("Error al conectar la base de datos");
            }
        }


    }
}
