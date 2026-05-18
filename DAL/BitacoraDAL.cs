using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Servicios;
using BE;
using System.Net;
using System.Reflection;

namespace DAL
{
    public class BitacoraDAL
    {
        ConexionBD conexion = new ConexionBD();
        public  bool RegistroBitacora(int idBitacora, int dni, string accion, DateTime fechaHora, 
            string modulo, string criticidad)
        {
            bool exito = false;

            using(SqlConnection conexionSql = conexion.ValidarConexion())
            {
                string consulta = "INSERT INTO Bitacora (DNI, Accion, " +
                    "FechaHora, Modulo, Criticidad) VALUES (@DNI, @Accion, @FechaHora, " +
                    "@Modulo, @Criticidad)";

                using (SqlCommand comando = new SqlCommand(consulta, conexionSql))
                {
                    comando.Parameters.AddWithValue("DNI", dni);
                    comando.Parameters.AddWithValue("Accion", accion);
                    comando.Parameters.AddWithValue("FechaHora", fechaHora);
                    comando.Parameters.AddWithValue("Modulo", modulo);
                    comando.Parameters.AddWithValue("Criticidad", criticidad);

                    try
                    {
                        conexionSql.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if(filasAfectadas > 0)
                        {
                            exito = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al registrar en la bitacora " + ex.Message);
                    }
                }
            }
            return exito;
            
        }

        public List<Bitacora> ListarBitacora()
        {
            List<Bitacora> listBitacora = new List<Bitacora>();

            using(SqlConnection conexionSql2 = conexion.ValidarConexion())
            {
                string consulta2 = "SELECT b.IdBitacora, b.DNI, u.NombreUsuario, b.Accion," +
                    " b.FechaHora, b.Modulo, b.Criticidad" +
                    " FROM Bitacora b INNER JOIN Usuarios u ON b.DNI = u.DNI" +
                    " ORDER BY b.FechaHora DESC"; //El DESC es para ver lo mas nuevo arriba

                using(SqlCommand comando2 = new SqlCommand(consulta2, conexionSql2))
                {
                    conexionSql2.Open();
                    using (SqlDataReader reader = comando2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bitacora bitacora = new Bitacora();
                            bitacora.IdBitacora = int.Parse(reader["IdBitacora"].ToString());
                            bitacora.DNI = int.Parse(reader["DNI"].ToString());
                            bitacora.Usuario = reader["NombreUsuario"].ToString();
                            bitacora.Accion = reader["Accion"].ToString();
                            bitacora.Modulo = reader["Modulo"].ToString();
                            bitacora.Criticidad = reader["Criticidad"].ToString();
                            bitacora.FechaHora = Convert.ToDateTime(reader["FechaHora"].ToString());

                            listBitacora.Add(bitacora);
                        }
                    }
                }
            }
            return listBitacora;
        }
    }
}
