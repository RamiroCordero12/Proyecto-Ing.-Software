using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class BitacoraDAL
    {
        ConexionBD conexion = new ConexionBD();

        public bool RegistroBitacora(int idBitacora, int dni, string accion, DateTime fechaHora,
            string modulo, string criticidad)
        {
            bool exito = false;

            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta =
                    "INSERT INTO Bitacora (DNI, Accion, FechaHora, Modulo, Criticidad) " +
                    "VALUES (@DNI, @Accion, @FechaHora, @Modulo, @Criticidad)";

                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    comando.Parameters.AddWithValue("@DNI", dni);
                    comando.Parameters.AddWithValue("@Accion", (object)(accion ?? string.Empty));
                    comando.Parameters.AddWithValue("@FechaHora", fechaHora);
                    comando.Parameters.AddWithValue("@Modulo", (object)(modulo ?? string.Empty));
                    comando.Parameters.AddWithValue("@Criticidad", (object)(criticidad ?? string.Empty));

                    try
                    {
                        conexionSql.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                            exito = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al registrar en la bitacora: " + ex.Message);
                    }
                }
            }
            return exito;
        }

        public List<Bitacora> ListarBitacora()
        {
            List<Bitacora> listBitacora = new List<Bitacora>();

            using (SqlConnection conexionSql2 = conexion.ValidarConexion())
            {
                // ISNULL guards every nullable column at the SQL level.
                // FechaHora is converted to a string so we fully control parsing.
                // LEFT JOIN keeps rows even when the DNI has no matching user.
                string consulta2 =
                    "SELECT b.IdBitacora, " +
                    "       b.DNI, " +
                    "       ISNULL(u.NombreUsuario, '(desconocido)') AS NombreUsuario, " +
                    "       ISNULL(b.Accion,     '')   AS Accion, " +
                    "       ISNULL(CONVERT(nvarchar(30), b.FechaHora, 120), '') AS FechaHora, " +
                    "       ISNULL(b.Modulo,     '')   AS Modulo, " +
                    "       ISNULL(b.Criticidad, '')   AS Criticidad " +
                    "FROM Bitacora b " +
                    "LEFT JOIN Usuarios u ON b.DNI = u.DNI " +
                    "ORDER BY b.FechaHora DESC";

                using (SqlCommand comando2 = new SqlCommand(consulta2, conexionSql2))
                {
                    conexionSql2.Open();
                    using (SqlDataReader reader = comando2.ExecuteReader())
                    {
                        int rowNum = 0;
                        while (reader.Read())
                        {
                            rowNum++;
                            try
                            {
                                Bitacora bitacora = new Bitacora();

                                bitacora.IdBitacora = Convert.ToInt32(reader["IdBitacora"]);
                                bitacora.DNI = Convert.ToInt32(reader["DNI"]);
                                bitacora.Usuario = reader["NombreUsuario"].ToString();
                                bitacora.Accion = reader["Accion"].ToString();
                                bitacora.Modulo = reader["Modulo"].ToString();
                                bitacora.Criticidad = reader["Criticidad"].ToString();

                                string fechaStr = reader["FechaHora"].ToString();
                                bitacora.FechaHora = string.IsNullOrEmpty(fechaStr)
                                    ? DateTime.MinValue
                                    : DateTime.Parse(fechaStr);

                                listBitacora.Add(bitacora);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(
                                    string.Format("Error en fila {0}: {1}", rowNum, ex.Message));
                            }
                        }
                    }
                }
            }
            return listBitacora;
        }

        public bool LimpiarBitacora()
        {
            using (SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "DELETE FROM Bitacora";
                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    conexionSql.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}