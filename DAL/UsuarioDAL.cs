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
                               "Estado, Email, IdRol, DigitoVerificador) " +
                "VALUES (@DNI, @Nombre, @Apellido, @NombreUsuario, @Contrasena, 1, @Email, @IdRol, @DigitoVerificador)";

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    comando.Parameters.AddWithValue("@DNI", usuario.DNI);
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    comando.Parameters.AddWithValue("@Email", usuario.Email);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    comando.Parameters.AddWithValue("@DigitoVerificador", DigitoVerificadorHelper.Calcular(usuario.DNI, usuario.NombreUsuario));

                    conexionSql.Open();

                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public List<Usuario> ListarUsuario()
        {
            List<Usuario> list = new List<Usuario>();

            string consulta2 = "SELECT DNI, Nombre, Apellido, NombreUsuario, Contrasena, Email, IdRol, " +
                                "Estado, IntentosFallidos, Lenguaje, DigitoVerificador FROM dbo.Usuarios";

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                using (SqlCommand comando2 = new SqlCommand(consulta2, conexionSql))
                {
                    conexionSql.Open();

                    using (SqlDataReader reader = comando2.ExecuteReader())
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
                            usuario.IdRol = int.Parse(reader["IdRol"].ToString());
                            usuario.Estado = reader["Estado"] != DBNull.Value && (bool)reader["Estado"];
                            usuario.IntentosFallidos = reader["IntentosFallidos"] != DBNull.Value ? int.Parse(reader["IntentosFallidos"].ToString()) : 0;
                            usuario.Lenguaje = reader["Lenguaje"] != DBNull.Value ? int.Parse(reader["Lenguaje"].ToString()) : 0;
                            usuario.DigitoVerificador = reader["DigitoVerificador"] != DBNull.Value ? int.Parse(reader["DigitoVerificador"].ToString()) : 0;

                            list.Add(usuario);
                        }
                    }
                }
            }
            return list;
        }

        public bool DeshabilitarUsuario(int dniUsuario)
        {
            bool exito = false;

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "UPDATE dbo.Usuarios SET Estado = 0 WHERE DNI = @DNI";

                using (SqlCommand comando3 = new SqlCommand(consulta, conexionSql))
                {
                    comando3.Parameters.AddWithValue("@DNI", dniUsuario);

                    conexionSql.Open();

                    int filasAfectadas = comando3.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            return exito;
        }

        public bool HabilitarUsuario(int dniUsuario)
        {
            bool exito = false;

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "UPDATE dbo.Usuarios SET Estado = 1, IntentosFallidos = 0 WHERE DNI = @DNI";

                using (SqlCommand comando4 = new SqlCommand(consulta, conexionSql))
                {
                    comando4.Parameters.AddWithValue("@DNI", dniUsuario);

                    conexionSql.Open();

                    int filasAfectadas = comando4.ExecuteNonQuery();

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
            bool resultado = false;

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "UPDATE Usuarios SET DNI = @DniNuevo, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, IdRol = @IdRol, " +
                                        "NombreUsuario = @NombreUsuario, Lenguaje = @Lenguaje, Contrasena = @Contrasena, DigitoVerificador = @DigitoVerificador WHERE DNI = @DniViejo";

                using (SqlCommand comando4 = new SqlCommand(consulta, conexionSql))
                {
                    comando4.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando4.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando4.Parameters.AddWithValue("@Email", usuario.Email);
                    comando4.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    comando4.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando4.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    comando4.Parameters.AddWithValue("@DniNuevo", usuario.DNI);
                    comando4.Parameters.AddWithValue("@DniViejo", dniViejo);
                    comando4.Parameters.AddWithValue("@Lenguaje", usuario.Lenguaje);
                    comando4.Parameters.AddWithValue("@DigitoVerificador", DigitoVerificadorHelper.Calcular(usuario.DNI, usuario.NombreUsuario));

                    conexionSql.Open();

                    int filasSeleccionadas = comando4.ExecuteNonQuery();

                    if (filasSeleccionadas > 0)
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

            string consulta = "SELECT DNI, NombreUsuario, Contrasena, Estado, Email, IdRol FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario AND Contrasena = @Contrasena AND Estado = 1";

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    comando.Parameters.AddWithValue("@Contrasena", contrasena);

                    conexionSql.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuarioLogueado = new Usuario
                            {
                                DNI = int.Parse(reader["DNI"].ToString()),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                Contrasena = reader["Contrasena"].ToString(),
                                Email = reader["Email"].ToString(),
                                IdRol = reader["IdRol"] != DBNull.Value ? int.Parse(reader["IdRol"].ToString()) : 0,
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
            string consulta = "SELECT DNI, NombreUsuario, Contrasena, Estado, Email, IdRol, IntentosFallidos, Lenguaje, DigitoVerificador FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario";
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
                            IdRol = int.Parse(reader["IdRol"].ToString()),
                            Estado = reader["Estado"] != DBNull.Value && (bool)reader["Estado"],
                            IntentosFallidos = reader["IntentosFallidos"] != DBNull.Value ? int.Parse(reader["IntentosFallidos"].ToString()) : 0,
                            Lenguaje = int.Parse(reader["Lenguaje"].ToString()),
                            DigitoVerificador = int.Parse(reader["DigitoVerificador"].ToString()),
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

        public bool CambiarLenguaje(int dni, int lenguaje)
        {
            bool resultado = false;
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "UPDATE Usuarios SET Lenguaje = @Lenguaje WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(consulta, conexionSql))
                {
                    cmd.Parameters.AddWithValue("@Lenguaje", lenguaje);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    conexionSql.Open();
                    int filas = cmd.ExecuteNonQuery();
                    resultado = filas > 0;
                }
            }
            return resultado;
        }
    }

}

