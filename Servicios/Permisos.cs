using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace Servicios
{
    public class Permisos
    {
        private readonly RolBE _rol;

        public Permisos(RolBE rolConPermisos)
        {
            _rol = rolConPermisos ?? throw new ArgumentNullException(nameof(rolConPermisos));
        }

        /// <summary>True if the role (directly or via a family) grants this patent.</summary>
        public bool Tiene(int idPatente) => _rol.TienePatente(idPatente);

        /// <summary>True if the role grants ANY of the given patents.</summary>
        public bool TieneAlguna(params int[] idsPatentes)
        {
            var todas = _rol.ObtenerTodasLasPatentes();
            return idsPatentes.Any(todas.Contains);
        }

        /// <summary>True if the role grants ALL of the given patents.</summary>
        public bool TieneTodas(params int[] idsPatentes)
        {
            var todas = _rol.ObtenerTodasLasPatentes();
            return idsPatentes.All(todas.Contains);
        }

        public HashSet<int> TodasLasPatentes() => _rol.ObtenerTodasLasPatentes();
    }
}

