using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Servicios;

namespace DAL
{
    public class UsuarioDAL
    {
        ConexionBD conexion = new ConexionBD();

        public bool CrearUsuario(Usuario usuario)
        {
            //Consulta SQL
            string consulta = "INSERT INTO Usuarios (DNI, Nombre, Apellido, NombreUsuario, Contrasena, " +
                "Estado, Email, Rol) " +
                "VALUES (@DNI, @Nombre, @Apellido, @NombreUsuario, @Contrasena, 1, @Email, @Rol)";

            //El uso de using hace que la conexion se abra y se cierra 
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                //Agarramos el comando SQL que vamos a enviar
                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    //Estos son los parametros
                    comando.Parameters.AddWithValue("DNI", usuario.DNI);
                    comando.Parameters.AddWithValue("Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("Contrasena", usuario.Contrasena);
                    comando.Parameters.AddWithValue("Email", usuario.Email);
                    comando.Parameters.AddWithValue("Rol", usuario.Rol);

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

        public List<Usuario> ListarUsuario()
        {
            //Creamos una lista con lo puesto en UsuarioBE
            List<Usuario> list = new List<Usuario>();

            //Creamos la consulta que seleccione los atributos de la tabla usuarios
            //SELECT --> Selecciona 
            //FROM --> De
            //SELECT ..... FROM --> Selecciona ... De
            string consulta2 = "SELECT DNI, Nombre, Apellido, NombreUsuario, Contrasena, Email, Rol, Estado, IntentosFallidos FROM dbo.Usuarios";

            using(SqlConnection conexionSql = conexion.ValidarConexion())
            {                         
                    using(SqlCommand comando2 = new SqlCommand(consulta2, conexionSql))
                    {
                    //Abrimos la conexion a la base de datos
                        conexionSql.Open();

                    //El executeReader lee la base de datos
                        using(SqlDataReader reader = comando2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.DNI = int.Parse(reader["DNI"].ToString());
                                usuario.Nombre = reader["Nombre"].ToString();
                                usuario.Apellido = reader["Apellido"].ToString();
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.Contrasena = reader["Contrasena"].ToString();
                                usuario.Email = reader["Email"].ToString();
                                usuario.Rol = int.Parse(reader["Rol"].ToString());               
                                usuario.Estado = bool.Parse(reader["Estado"].ToString());      
                                usuario.IntentosFallidos = int.Parse(reader["IntentosFallidos"].ToString());
                            //Agrega los datos a la lista
                                list.Add(usuario);
                            }
                        }
                    }
                                
            }
            return list;
        }

        public bool DeshabilitarUsuario(int dniUsuario)
        {
            //Creamos una variable booleana para confirmar 
            bool exito = false;

            //Conectamos
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                //Creamos la consulta que modifique de usuario a estado en 0
                //UPDATE --> Modifica de
                //SET --> Asigna valores
                //WHERE --> Donde
                string consulta = "UPDATE dbo.Usuarios SET Estado = 0 WHERE DNI = @DNI";

                using(SqlCommand comando3 = new SqlCommand(consulta, conexionSql))
                {
                    //Agarramos el parametro de IdUsuario
                    comando3.Parameters.AddWithValue("@DNI", dniUsuario);

                    conexionSql.Open();

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

        public bool HabilitarUsuario(int dniUsuario)
        {
            //Creamos una variable booleana para confirmar 
            bool exito = false;

            //Conectamos
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                //Creamos la consulta que modifique de usuario a estado en 0
                //UPDATE --> Modifica de
                //SET --> Asigna valores
                //WHERE --> Donde
                string consulta = "UPDATE dbo.Usuarios SET Estado = 1, IntentosFallidos = 0 WHERE DNI = @DNI";

                using (SqlCommand comando4 = new SqlCommand(consulta, conexionSql))
                {
                    //Agarramos el parametro de IdUsuario
                    comando4.Parameters.AddWithValue("@DNI", dniUsuario);

                    conexionSql.Open();

                    int filasAfectadas = comando4.ExecuteNonQuery();

                    //Si filasAfectadas es mayor a 0 entonces...
                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            return exito;
        }

        public bool ModificarUsuario(Usuario usuario, int dniViejo)
        {
            //Variable booleana para confirmar el resultado
            bool resultado = false;

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                //Creamos la consulta que modifique el usuario
                //UPDATE de usuarios el nombre de usuario y la contrasena done el
                //el Id seleccionado sea el mismo que el de la base de datos
                string consulta = "UPDATE Usuarios SET DNI = @DniNuevo, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Rol = @Rol, " +
                    "NombreUsuario = @NombreUsuario, Contrasena = @Contrasena WHERE DNI = @DniViejo";

                using(SqlCommand comando4 = new SqlCommand(consulta, conexionSql))
                {
                    //Agarramos los parametros que los vamos a modificar
                    comando4.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando4.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando4.Parameters.AddWithValue("@Email", usuario.Email);
                    comando4.Parameters.AddWithValue("@Rol", usuario.Rol);
                    comando4.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando4.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    comando4.Parameters.AddWithValue("@DniNuevo", usuario.DNI);
                    comando4.Parameters.AddWithValue("@DniViejo", dniViejo);


                    conexionSql.Open();

                    int filasSeleccionadas = comando4.ExecuteNonQuery();

                    if(filasSeleccionadas > 0)
                    {
                        resultado = true;
                    }
                }
            }
            return resultado;
        }

        public Usuario Login(string nombreUsuario, string contrasena)
        {
            Usuario usuarioLogueado = null;

            string consulta = "SELECT DNI, NombreUsuario, Contrasena, Estado, Email, Rol FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario AND Contrasena = @Contrasena AND Estado = 1";

            using(SqlConnection conexionSql = conexion.ValidarConexion())
            {
                using(SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    comando.Parameters.AddWithValue("NombreUsuario", nombreUsuario);
                    comando.Parameters.AddWithValue("Contrasena", contrasena);

                    conexionSql.Open();

                    using(SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuarioLogueado = new Usuario
                            {
                                DNI = int.Parse(reader["DNI"].ToString()),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                Contrasena = reader["Contrasena"].ToString(),
                                Email = reader["Email"].ToString(),
                                Rol = int.Parse(reader["Rol"].ToString()),
                            };

                        }
                    }
                }
            }
            return usuarioLogueado;
        }
        public bool CambiarContrasena(int dni, string nuevaContrasenaHash)
        {
            bool resultado = false;
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "UPDATE Usuarios SET Contrasena = @Contrasena WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(consulta, conexionSql))
                {
                    cmd.Parameters.AddWithValue("@Contrasena", nuevaContrasenaHash);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    conexionSql.Open();
                    int filas = cmd.ExecuteNonQuery();
                    resultado = filas > 0;
                }
            }
            return resultado;
        }

        // Helper para conseguir el hash de la contraseña del usuario actual
        public string ObtenerContrasenaHash(int dni)
        {
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "SELECT Contrasena FROM Usuarios WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(consulta, conexionSql))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    conexionSql.Open();
                    var result = cmd.ExecuteScalar();
                    return result == null || result == DBNull.Value ? null : result.ToString();
                }
            }
        }

        public Usuario GetUsuarioByNombreUsuario(string nombreUsuario)
        {
            Usuario usuario = null;
            string consulta = "SELECT DNI, NombreUsuario, Contrasena, Estado, Email, Rol, IntentosFallidos FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario";

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
            {
                comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                conexionSql.Open();
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            DNI = int.Parse(reader["DNI"].ToString()),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Contrasena = reader["Contrasena"].ToString(),
                            Email = reader["Email"].ToString(),
                            Rol = int.Parse(reader["Rol"].ToString()),
                            Estado = reader["Estado"] != DBNull.Value && (bool)reader["Estado"],
                            IntentosFallidos = reader["IntentosFallidos"] != DBNull.Value ? int.Parse(reader["IntentosFallidos"].ToString()) : 0
                        };
                    }
                }
            }
            return usuario;
        }

        public bool ActualizarIntentosYEstado(int dni, int intentosFallidos, bool estado)
        {
            bool exito = false;
            string consulta = "UPDATE dbo.Usuarios SET IntentosFallidos = @IntentosFallidos, Estado = @Estado WHERE DNI = @DNI";

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
            {
                comando.Parameters.AddWithValue("@IntentosFallidos", intentosFallidos);
                comando.Parameters.AddWithValue("@Estado", estado ? 1 : 0);
                comando.Parameters.AddWithValue("@DNI", dni);

                conexionSql.Open();
                int filas = comando.ExecuteNonQuery();
                exito = filas > 0;
            }
            return exito;
        }

    }

}

