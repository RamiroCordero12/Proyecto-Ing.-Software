using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Servicios;

namespace BLL
{
    public class PatentesBLL
    {
        private readonly PatentesDAL _dal = new PatentesDAL();

        public List<PatenteComponent> ListarPatentes()
        {
            return _dal.ListarPatentes();
        }

        public PatenteComponent ObtenerPatente(int idPatente)
        {
            if (idPatente <= 0)
                throw new ArgumentException("ID de patente inválido.");
            return _dal.ObtenerPatente(idPatente);
        }

        public List<PatenteComponent> ListarPatentesPorFamilia(int idFamilia)
        {
            if (idFamilia <= 0)
                throw new ArgumentException("ID de familia inválido.");
            return _dal.ListarPatentesPorFamilia(idFamilia);
        }

        public List<PatenteComponent> ListarPatentesPorRol(int idRol)
        {
            if (idRol <= 0)
                throw new ArgumentException("ID de rol inválido.");
            return _dal.ListarPatentesPorRol(idRol);
        }
    }

}
