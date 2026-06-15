using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Servicios;

namespace BLL
{
    // =========================================================================
    //  FamiliasBLL
    // =========================================================================
    public class FamiliasBLL
    {
        private readonly FamiliasDAL _dal = new FamiliasDAL();
        private readonly BitacoraDAL _bitacora = new BitacoraDAL();

        public bool CrearFamilia(FamiliaComponent familia, List<int> idPatentes)
        {
            if (string.IsNullOrWhiteSpace(familia.Nombre))
                throw new ArgumentException("El nombre de la familia no puede estar vacío.");

            if (idPatentes == null || idPatentes.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una patente.");

            bool ok = _dal.CrearFamilia(familia, idPatentes);

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Familia creada: " + familia.Nombre,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }

        public List<FamiliaComponent> ListarFamilias()
        {
            return _dal.ListarFamilias();
        }

        public FamiliaComponent ObtenerFamilia(int idFamilia)
        {
            if (idFamilia <= 0)
                throw new ArgumentException("ID de familia inválido.");
            return _dal.ObtenerFamilia(idFamilia);
        }

        public bool ModificarFamilia(FamiliaComponent familia, List<int> idPatentes)
        {
            if (string.IsNullOrWhiteSpace(familia.Nombre))
                throw new ArgumentException("El nombre de la familia no puede estar vacío.");

            if (idPatentes == null || idPatentes.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una patente.");

            bool ok = _dal.ModificarFamilia(familia, idPatentes);

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Familia modificada: " + familia.Nombre,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }

        public bool EliminarFamilia(int idFamilia)
        {
            if (idFamilia <= 0)
                throw new ArgumentException("ID de familia inválido.");

            bool ok = _dal.EliminarFamilia(idFamilia);

            if (ok)
            {
                Bitacora b = new Bitacora();
                _bitacora.RegistroBitacora(
                    b.IdBitacora,
                    SessionManager.GetInstance.usuario.DNI,
                    b.Accion = "Familia eliminada ID: " + idFamilia,
                    DateTime.Now, "Gestor de Roles", b.Criticidad = "Alta");
            }

            return ok;
        }
    }
}
