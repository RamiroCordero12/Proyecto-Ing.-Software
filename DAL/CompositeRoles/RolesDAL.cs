using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace DAL
{
    public class RolesDAL
    {
        private readonly ConexionBD _cx = new ConexionBD();

        // ── Create role + link patents and families (transaction) ──────────
        public bool CrearRol(RolBE rol, List<int> idPatentes, List<int> idFamilias)
        {
            using (SqlConnection con = _cx.ValidarConexion())
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // Insert role
                        int idRol;
                        string sqlInsert =
                            "INSERT INTO Roles (NombreRol, Descripcion) " +
                            "OUTPUT INSERTED.IdRol " +
                            "VALUES (@Nombre, @Desc)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsert, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", rol.NombreRol);
                            cmd.Parameters.AddWithValue("@Desc",
                                string.IsNullOrEmpty(rol.Descripcion)
                                    ? (object)DBNull.Value
                                    : rol.Descripcion);
                            idRol = (int)cmd.ExecuteScalar();
                        }

                        // Link patents
                        foreach (int idP in idPatentes)
                        {
                            using (SqlCommand cmd2 = new SqlCommand(
                                "INSERT INTO Rol_Pat (IdRol, IdPatente) VALUES (@R, @P)",
                                con, tx))
                            {
                                cmd2.Parameters.AddWithValue("@R", idRol);
                                cmd2.Parameters.AddWithValue("@P", idP);
                                cmd2.ExecuteNonQuery();
                            }
                        }

                        // Link families
                        foreach (int idF in idFamilias)
                        {
                            using (SqlCommand cmd3 = new SqlCommand(
                                "INSERT INTO Rol_Fam (IdRol, IdFamilia) VALUES (@R, @F)",
                                con, tx))
                            {
                                cmd3.Parameters.AddWithValue("@R", idRol);
                                cmd3.Parameters.AddWithValue("@F", idF);
                                cmd3.ExecuteNonQuery();
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

        // ── List all roles (header only, no tree) ─────────────────────────
        public List<RolBE> ListarRoles()
        {
            var list = new List<RolBE>();
            string sql = "SELECT IdRol, NombreRol, Descripcion FROM Roles ORDER BY IdRol";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new RolBE
                        {
                            IdRol = (int)r["IdRol"],
                            NombreRol = r["NombreRol"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        });
                    }
                }
            }
            return list;
        }

        // ── Get full composite tree for one role ──────────────────────────
        public RolBE ObtenerRolConPermisos(int idRol)
        {
            RolBE rol = null;

            // Step 1: get role header
            string sqlHeader = "SELECT IdRol, NombreRol, Descripcion FROM Roles WHERE IdRol = @Id";
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sqlHeader, con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        rol = new RolBE
                        {
                            IdRol = (int)r["IdRol"],
                            NombreRol = r["NombreRol"].ToString(),
                            Descripcion = r["Descripcion"] != DBNull.Value
                                          ? r["Descripcion"].ToString()
                                          : string.Empty
                        };
                    }
                }
            }

            if (rol == null) return null;

            // Step 2: attach patents assigned directly to the role
            string sqlPatentes =
                "SELECT p.IdPatente, p.NombrePatente, p.Descripcion " +
                "FROM Patentes p " +
                "INNER JOIN Rol_Pat rp ON p.IdPatente = rp.IdPatente " +
                "WHERE rp.IdRol = @Id";

            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sqlPatentes, con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        rol.AgregarComponente(new PatenteComponent
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

            // Step 3: attach families (each family already contains its patents)
            string sqlFamilias =
                "SELECT f.IdFamilia, f.NombreFamilia, f.Descripcion AS DescFam, " +
                "       p.IdPatente, p.NombrePatente, p.Descripcion AS DescPat " +
                "FROM Familias f " +
                "INNER JOIN Rol_Fam rf ON f.IdFamilia = rf.IdFamilia " +
                "LEFT  JOIN Fam_Pat fp ON f.IdFamilia = fp.IdFamilia " +
                "LEFT  JOIN Patentes p ON fp.IdPatente = p.IdPatente " +
                "WHERE rf.IdRol = @Id " +
                "ORDER BY f.IdFamilia";

            var familiaDict = new Dictionary<int, FamiliaComponent>();
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(sqlFamilias, con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int idF = (int)r["IdFamilia"];
                        if (!familiaDict.ContainsKey(idF))
                        {
                            familiaDict[idF] = new FamiliaComponent
                            {
                                Id = idF,
                                Nombre = r["NombreFamilia"].ToString(),
                                Descripcion = r["DescFam"] != DBNull.Value
                                              ? r["DescFam"].ToString()
                                              : string.Empty
                            };
                        }
                        if (r["IdPatente"] != DBNull.Value)
                        {
                            familiaDict[idF].Agregar(new PatenteComponent
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

            foreach (var fam in familiaDict.Values)
                rol.AgregarComponente(fam);

            return rol;
        }

        // ── Update role header + replace links ────────────────────────────
        public bool ModificarRol(RolBE rol, List<int> idPatentes, List<int> idFamilias)
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
                            "UPDATE Roles SET NombreRol = @Nombre, Descripcion = @Desc " +
                            "WHERE IdRol = @Id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", rol.NombreRol);
                            cmd.Parameters.AddWithValue("@Desc",
                                string.IsNullOrEmpty(rol.Descripcion)
                                    ? (object)DBNull.Value
                                    : rol.Descripcion);
                            cmd.Parameters.AddWithValue("@Id", rol.IdRol);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete old patent links
                        using (SqlCommand del = new SqlCommand(
                            "DELETE FROM Rol_Pat WHERE IdRol = @Id", con, tx))
                        {
                            del.Parameters.AddWithValue("@Id", rol.IdRol);
                            del.ExecuteNonQuery();
                        }

                        // Delete old family links
                        using (SqlCommand del = new SqlCommand(
                            "DELETE FROM Rol_Fam WHERE IdRol = @Id", con, tx))
                        {
                            del.Parameters.AddWithValue("@Id", rol.IdRol);
                            del.ExecuteNonQuery();
                        }

                        // Re-insert patent links
                        foreach (int idP in idPatentes)
                        {
                            using (SqlCommand ins = new SqlCommand(
                                "INSERT INTO Rol_Pat (IdRol, IdPatente) VALUES (@R, @P)",
                                con, tx))
                            {
                                ins.Parameters.AddWithValue("@R", rol.IdRol);
                                ins.Parameters.AddWithValue("@P", idP);
                                ins.ExecuteNonQuery();
                            }
                        }

                        // Re-insert family links
                        foreach (int idF in idFamilias)
                        {
                            using (SqlCommand ins = new SqlCommand(
                                "INSERT INTO Rol_Fam (IdRol, IdFamilia) VALUES (@R, @F)",
                                con, tx))
                            {
                                ins.Parameters.AddWithValue("@R", rol.IdRol);
                                ins.Parameters.AddWithValue("@F", idF);
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

        // ── Delete a role ─────────────────────────────────────────────────
        public bool EliminarRol(int idRol)
        {
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Roles WHERE IdRol = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ── Get the IDs of patents directly assigned to a role ────────────
        public List<int> ObtenerIdPatentesPorRol(int idRol)
        {
            var list = new List<int>();
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT IdPatente FROM Rol_Pat WHERE IdRol = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add((int)r["IdPatente"]);
            }
            return list;
        }

        // ── Get the IDs of families assigned to a role ────────────────────
        public List<int> ObtenerIdFamiliasPorRol(int idRol)
        {
            var list = new List<int>();
            using (SqlConnection con = _cx.ValidarConexion())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT IdFamilia FROM Rol_Fam WHERE IdRol = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idRol);
                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add((int)r["IdFamilia"]);
            }
            return list;
        }
    }
}
