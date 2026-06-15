using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace DAL
{
    public class FamiliasDAL
    {
        private readonly ConexionBD _cx = new ConexionBD();

        // ── Create family + link patents (transaction) ────────────────────
        public bool CrearFamilia(FamiliaComponent familia, List<int> idPatentes)
        {
            using (SqlConnection con = _cx.ValidarConexion())
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // Insert the family and get the generated ID
                        int idFam;
                        string sqlInsert =
                            "INSERT INTO Familias (NombreFamilia, Descripcion) " +
                            "OUTPUT INSERTED.IdFamilia " +
                            "VALUES (@Nombre, @Desc)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsert, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", familia.Nombre);
                            cmd.Parameters.AddWithValue("@Desc",
                                string.IsNullOrEmpty(familia.Descripcion)
                                    ? (object)DBNull.Value
                                    : familia.Descripcion);

                            idFam = (int)cmd.ExecuteScalar();
                        }

                        // Link each selected patent
                        foreach (int idP in idPatentes)
                        {
                            using (SqlCommand cmd2 = new SqlCommand(
                                "INSERT INTO Fam_Pat (IdFamilia, IdPatente) VALUES (@F, @P)",
                                con, tx))
                            {
                                cmd2.Parameters.AddWithValue("@F", idFam);
                                cmd2.Parameters.AddWithValue("@P", idP);
                                cmd2.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // ── List all families with their child patents ────────────────────
        public List<FamiliaComponent> ListarFamilias()
        {
            var dict = new Dictionary<int, FamiliaComponent>();

            string sql =
                "SELECT f.IdFamilia, f.NombreFamilia, f.Descripcion, " +
                "       p.IdPatente, p.NombrePatente, p.Descripcion AS DescPat " +
                "FROM Familias f " +
                "LEFT JOIN Fam_Pat fp ON f.IdFamilia = fp.IdFamilia " +
                "LEFT JOIN Patentes p  ON fp.IdPatente = p.IdPatente " +
                "ORDER BY f.IdFamilia";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int idF = (int)r["IdFamilia"];

                        if (!dict.ContainsKey(idF))
                        {
                            dict[idF] = new FamiliaComponent
                            {
                                Id = idF,
                                Nombre = r["NombreFamilia"].ToString(),
                                Descripcion = r["Descripcion"] != DBNull.Value
                                              ? r["Descripcion"].ToString()
                                              : string.Empty
                            };
                        }

                        if (r["IdPatente"] != DBNull.Value)
                        {
                            dict[idF].Agregar(new PatenteComponent
                            {
                                Id = (int)r["IdPatente"],
                                Nombre = r["NombrePatente"].ToString(),
                                Descripcion = r["DescPat"] != DBNull.Value
                                              ? r["DescPat"].ToString()
                                              : string.Empty
                            });
                        }
                    }
                }
            }

            return new List<FamiliaComponent>(dict.Values);
        }

        // ── Get a single family by ID (with children) ─────────────────────
        public FamiliaComponent ObtenerFamilia(int idFamilia)
        {
            FamiliaComponent familia = null;

            string sql =
                "SELECT f.IdFamilia, f.NombreFamilia, f.Descripcion, " +
                "       p.IdPatente, p.NombrePatente, p.Descripcion AS DescPat " +
                "FROM Familias f " +
                "LEFT JOIN Fam_Pat fp ON f.IdFamilia = fp.IdFamilia " +
                "LEFT JOIN Patentes p  ON fp.IdPatente = p.IdPatente " +
                "WHERE f.IdFamilia = @Id";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", idFamilia);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        if (familia == null)
                        {
                            familia = new FamiliaComponent
                            {
                                Id = (int)r["IdFamilia"],
                                Nombre = r["NombreFamilia"].ToString(),
                                Descripcion = r["Descripcion"] != DBNull.Value
                                              ? r["Descripcion"].ToString()
                                              : string.Empty
                            };
                        }

                        if (r["IdPatente"] != DBNull.Value)
                        {
                            familia.Agregar(new PatenteComponent
                            {
                                Id = (int)r["IdPatente"],
                                Nombre = r["NombrePatente"].ToString(),
                                Descripcion = r["DescPat"] != DBNull.Value
                                              ? r["DescPat"].ToString()
                                              : string.Empty
                            });
                        }
                    }
                }
            }

            return familia;
        }

        // ── Update family name/description and replace its patents ─────────
        public bool ModificarFamilia(FamiliaComponent familia, List<int> idPatentes)
        {
            using (SqlConnection con = _cx.ValidarConexion())
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // Update header
                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Familias SET NombreFamilia = @Nombre, Descripcion = @Desc " +
                            "WHERE IdFamilia = @Id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", familia.Nombre);
                            cmd.Parameters.AddWithValue("@Desc",
                                string.IsNullOrEmpty(familia.Descripcion)
                                    ? (object)DBNull.Value
                                    : familia.Descripcion);
                            cmd.Parameters.AddWithValue("@Id", familia.Id);
                            cmd.ExecuteNonQuery();
                        }

                        // Replace patent links
                        using (SqlCommand del = new SqlCommand(
                            "DELETE FROM Fam_Pat WHERE IdFamilia = @Id", con, tx))
                        {
                            del.Parameters.AddWithValue("@Id", familia.Id);
                            del.ExecuteNonQuery();
                        }

                        foreach (int idP in idPatentes)
                        {
                            using (SqlCommand ins = new SqlCommand(
                                "INSERT INTO Fam_Pat (IdFamilia, IdPatente) VALUES (@F, @P)",
                                con, tx))
                            {
                                ins.Parameters.AddWithValue("@F", familia.Id);
                                ins.Parameters.AddWithValue("@P", idP);
                                ins.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // ── Delete a family (cascade removes Fam_Pat rows) ───────────────
        public bool EliminarFamilia(int idFamilia)
        {
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Familias WHERE IdFamilia = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idFamilia);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ── List families assigned to a role ──────────────────────────────
        public List<FamiliaComponent> ListarFamiliasPorRol(int idRol)
        {
            var dict = new Dictionary<int, FamiliaComponent>();

            string sql =
                "SELECT f.IdFamilia, f.NombreFamilia, f.Descripcion, " +
                "       p.IdPatente, p.NombrePatente, p.Descripcion AS DescPat " +
                "FROM Familias f " +
                "INNER JOIN Rol_Fam rf ON f.IdFamilia = rf.IdFamilia " +
                "LEFT  JOIN Fam_Pat fp ON f.IdFamilia = fp.IdFamilia " +
                "LEFT  JOIN Patentes p ON fp.IdPatente = p.IdPatente " +
                "WHERE rf.IdRol = @IdRol " +
                "ORDER BY f.IdFamilia";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IdRol", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int idF = (int)r["IdFamilia"];
                        if (!dict.ContainsKey(idF))
                        {
                            dict[idF] = new FamiliaComponent
                            {
                                Id = idF,
                                Nombre = r["NombreFamilia"].ToString(),
                                Descripcion = r["Descripcion"] != DBNull.Value
                                              ? r["Descripcion"].ToString()
                                              : string.Empty
                            };
                        }
                        if (r["IdPatente"] != DBNull.Value)
                        {
                            dict[idF].Agregar(new PatenteComponent
                            {
                                Id = (int)r["IdPatente"],
                                Nombre = r["NombrePatente"].ToString(),
                                Descripcion = r["DescPat"] != DBNull.Value
                                              ? r["DescPat"].ToString()
                                              : string.Empty
                            });
                        }
                    }
                }
            }

            return new List<FamiliaComponent>(dict.Values);
        }
    }
}
