using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class UsuarioDAL
    {
        ConexionBD conexion = new ConexionBD();

        public bool CrearUsuario(string nombre, string contrasena)
        {
            //Consulta SQL
            string consulta = "INSERT INTO dbo.Usuarios (NombreUsuario, Contrasena) VALUES (@NombreUsuario, @Contrasena)"; 

            //El uso de using hace que la conexion se abra y se cierra 
            using(SqlConnection conexionSql = conexion.ValidacionConexion())
            {
                try
                {
                    //Agarramos el comando SQL que vamos a enviar
                    using(SqlCommand comando = new SqlCommand(consulta , conexionSql))
                    {
                        //Estos son los parametros
                        comando.Parameters.AddWithValue("NombreUsuario", nombre);
                        comando.Parameters.AddWithValue("Contrasena", contrasena);

                        //Antes de ejecutar abrimos la conexion
                        conexionSql.Open();

                        
                        int filasAfectadas = comando.ExecuteNonQuery();
                        //ExecuteNonQuery: ExecuteNonQuery() se usa para comandos
                        //que "hacen algo" en la base de datos pero no devuelven una tabla de
                        //resultados para leer

                        //Si afecto a alguna fila el usuario se creo con exito
                        return filasAfectadas > 0;
                    }
                }
                //Validacion
                catch
                {
                    throw new Exception("Error al crear un nuevo usuario");
                }
            }
        }

        public List<UsuarioBE> ListarUsuario()
        {
            List<UsuarioBE> list = new List<UsuarioBE>();

            string consulta2 = "SELECT IdUsuario, NombreUsuario, Contrasena, Estado FROM dbo.Usuarios";

            using(SqlConnection conexionSql2 = conexion.ValidacionConexion())
            {
                
                
                    using(SqlCommand comando2 = new SqlCommand(consulta2, conexionSql2))
                    {
                        conexionSql2.Open();

                        using(SqlDataReader reader = comando2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsuarioBE usuario = new UsuarioBE();

                                usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.Contrasena = reader["Contrasena"].ToString();
                                usuario.Estado = Convert.ToBoolean(reader["Estado"]);

                                list.Add(usuario);
                            }
                        }
                    }
                                
            }
            return list;
        }
    }
}
