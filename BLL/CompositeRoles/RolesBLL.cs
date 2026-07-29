using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BE;
using Servicios;

namespace BLL
{
    // =========================================================================
    //  RolesBLL
    // =========================================================================
    public class RolesBLL
    {
        private readonly RolesDAL _dal = new RolesDAL();
        private readonly BitacoraDAL _bitacora = new BitacoraDAL();

        public bool CrearRol(RolBE rol, List<int> idPatentes, List<int> idFamilias)
        {
            if (string.IsNullOrWhiteSpace(rol.NombreRol))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            if ((idPatentes == null || idPatentes.Count == 0) &&
                (idFamilias == null || idFamilias.Count == 0))
                throw new ArgumentException("Debe asignar al menos una patente o familia al rol.");

            bool ok = _dal.CrearRol(rol,
                idPatentes ?? new List<int>(),
                idFamilias ?? new List<int>());

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Rol creado: " + rol.NombreRol,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }

        public List<RolBE> ListarRoles()
        {
            return _dal.ListarRoles();
        }

        public RolBE ObtenerRolConPermisos(int idRol)
        {
            if (idRol <= 0)
                throw new ArgumentException("ID de rol inválido.");
            return _dal.ObtenerRolConPermisos(idRol);
        }

        public bool ModificarRol(RolBE rol, List<int> idPatentes, List<int> idFamilias)
        {
            if (string.IsNullOrWhiteSpace(rol.NombreRol))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            if ((idPatentes == null || idPatentes.Count == 0) &&
                (idFamilias == null || idFamilias.Count == 0))
                throw new ArgumentException("Debe asignar al menos una patente o familia al rol.");

            bool ok = _dal.ModificarRol(rol,
                idPatentes ?? new List<int>(),
                idFamilias ?? new List<int>());

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Rol modificado: " + rol.NombreRol,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }

        public bool EliminarRol(int idRol)
        {
            if (idRol <= 0)
                throw new ArgumentException("ID de rol inválido.");

            bool ok = _dal.EliminarRol(idRol);

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Rol eliminado ID: " + idRol,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }

        public List<int> ObtenerIdPatentesPorRol(int idRol)
            => _dal.ObtenerIdPatentesPorRol(idRol);

        public List<int> ObtenerIdFamiliasPorRol(int idRol)
            => _dal.ObtenerIdFamiliasPorRol(idRol);
    }
}
