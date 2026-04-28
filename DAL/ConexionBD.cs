using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class ConexionBD
    {
        //Cadena que conecta la base de datos con el codigo
        string cadenaConexion = @"Data Source=DESKTOP-F64FVOG\SQLEXPRESS;Initial Catalog = Usuarios; Integrated Security = True; Encrypt=False";
    
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
