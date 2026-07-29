using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class PatentesDAL
    {
        private readonly ConexionBD _cx = new ConexionBD();

        // ── List all patents ──────────────────────────────────────────────
        public List<PatenteComponent> ListarPatentes()
        {
            var list = new List<PatenteComponent>();
            string sql = "SELECT IdPatente, NombrePatente, Descripcion FROM Patentes ORDER BY IdPatente";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new PatenteComponent
                        {
                            Id = (int)r["IdPatente"],
                            Nombre = r["NombrePatente"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // ── Get single patent by ID ───────────────────────────────────────
        public PatenteComponent ObtenerPatente(int idPatente)
        {
            string sql = "SELECT IdPatente, NombrePatente, Descripcion " +
                         "FROM Patentes WHERE IdPatente = @Id";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", idPatente);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new PatenteComponent
                        {
                            Id = (int)r["IdPatente"],
                            Nombre = r["NombrePatente"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        };
                    }
                }
            }
            return null;
        }

        // ── Get patents belonging to a specific family ────────────────────
        public List<PatenteComponent> ListarPatentesPorFamilia(int idFamilia)
        {
            var list = new List<PatenteComponent>();
            string sql =
                "SELECT p.IdPatente, p.NombrePatente, p.Descripcion " +
                "FROM Patentes p " +
                "INNER JOIN Fam_Pat fp ON p.IdPatente = fp.IdPatente " +
                "WHERE fp.IdFamilia = @IdFamilia " +
                "ORDER BY p.IdPatente";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IdFamilia", idFamilia);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new PatenteComponent
                        {
                            Id = (int)r["IdPatente"],
                            Nombre = r["NombrePatente"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // ── Get patents assigned directly to a role ───────────────────────
        public List<PatenteComponent> ListarPatentesPorRol(int idRol)
        {
            var list = new List<PatenteComponent>();
            string sql =
                "SELECT p.IdPatente, p.NombrePatente, p.Descripcion " +
                "FROM Patentes p " +
                "INNER JOIN Rol_Pat rp ON p.IdPatente = rp.IdPatente " +
                "WHERE rp.IdRol = @IdRol " +
                "ORDER BY p.IdPatente";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IdRol", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new PatenteComponent
                        {
                            Id = (int)r["IdPatente"],
                            Nombre = r["NombrePatente"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        });
                    }
                }
            }
            return list;
        }
    }
}
