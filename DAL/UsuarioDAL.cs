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
            string consulta = "INSERT INTO dbo.Usuarios (NombreUsuario, Contrasena, Estado) VALUES (@NombreUsuario, @Contrasena, @Estado)";

            //El uso de using hace que la conexion se abra y se cierra 
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                //Agarramos el comando SQL que vamos a enviar
                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    //Estos son los parametros
                    comando.Parameters.AddWithValue("NombreUsuario", nombre);
                    comando.Parameters.AddWithValue("Contrasena", contrasena);

                    comando.Parameters.AddWithValue("Estado", true);
                    //Antes de ejecutar abrimos la conexion
                    conexionSql.Open();


                    int filasAfectadas = comando.ExecuteNonQuery();
                    //ExecuteNonQuery: Metodo que ejecuta una sentencias
                    //SQL que no devuelven conjunto de datos (INSERT, UPDATE
                    //DELETE o CREATE.

                    //Si afecto a alguna fila el usuario se creo con exito
                    return filasAfectadas > 0;
                }
            }                
            
        }

        public List<UsuarioBE> ListarUsuario()
        {
            //Creamos una lista con lo puesto en UsuarioBE
            List<UsuarioBE> list = new List<UsuarioBE>();

            //Creamos la consulta que seleccione los atributos de la tabla usuarios
            //SELECT --> Selecciona 
            //FROM --> De
            //SELECT ..... FROM --> Selecciona ... De
            string consulta2 = "SELECT IdUsuario, NombreUsuario, Contrasena, Estado FROM dbo.Usuarios";

            using(SqlConnection conexionSql2 = conexion.ValidarConexion())
            {                         
                    using(SqlCommand comando2 = new SqlCommand(consulta2, conexionSql2))
                    {
                    //Abrimos la conexion a la base de datos
                        conexionSql2.Open();

                    //El executeReader lee la base de datos
                        using(SqlDataReader reader = comando2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsuarioBE usuario = new UsuarioBE();

                                usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.Contrasena = reader["Contrasena"].ToString();
                                usuario.Estado = Convert.ToBoolean(reader["Estado"]);
                            
                            //Agrega los datos a la lista
                                list.Add(usuario);
                            }
                        }
                    }
                                
            }
            return list;
        }

        public bool DeshabilitarUsuario(int idUsuario)
        {
            //Creamos una variable booleana para confirmar 
            bool exito = false;

            //Conectamos
            using (SqlConnection conexionSql3 = conexion.ValidarConexion())
            {
                //Creamos la consulta que modifique de usuario a estado en 0
                //UPDATE --> Modifica de
                //SET --> Asigna valores
                //WHERE --> Donde
                string consulta = "UPDATE dbo.Usuarios SET Estado = 0 WHERE IdUsuario = @IdUsuario";

                using(SqlCommand comando3 = new SqlCommand(consulta, conexionSql3))
                {
                    //Agarramos el parametro de IdUsuario
                    comando3.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexionSql3.Open();

                    int filasAfectadas = comando3.ExecuteNonQuery();

                    //Si filasAfectadas es mayor a 0 entonces...
                    if(filasAfectadas > 0)
                    {
                        exito = true;                    
                    }
                }
            }
            return exito;
        }

        public bool ModificarUsuario(UsuarioBE usuarioBE)
        {
            //Variable booleana para confirmar el resultado
            bool resultado = false;

            using (SqlConnection conexionSql4 = conexion.ValidarConexion())
            {
                //Creamos la consulta que modifique el usuario
                //UPDATE de usuarios el nombre de usuario y la contrasena done el
                //el Id seleccionado sea el mismo que el de la base de datos
                string consulta = "UPDATE Usuarios SET NombreUsuario = @NombreUsuario, Contrasena = @Contrasena WHERE IdUsuario = @IdUsuario";

                using(SqlCommand comando4 = new SqlCommand(consulta, conexionSql4))
                {
                    //Agarramos los parametros que los vamos a modificar
                    comando4.Parameters.AddWithValue("@NombreUsuario", usuarioBE.NombreUsuario);
                    comando4.Parameters.AddWithValue("@Contrasena", usuarioBE.Contrasena);
                    comando4.Parameters.AddWithValue("@IdUsuario", usuarioBE.IdUsuario);

                    conexionSql4.Open();

                    int filasSeleccionadas = comando4.ExecuteNonQuery();

                    if(filasSeleccionadas > 0)
                    {
                        resultado = true;
                    }
                }
            }
            return resultado;
        }
    }
}
