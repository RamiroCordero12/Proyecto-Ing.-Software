using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class RolBE
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }

        private readonly List<IComponentePermiso> _componentes = new List<IComponentePermiso>();

        // ── Component management ──────────────────────────────────────────
        public void AgregarComponente(IComponentePermiso componente)
        {
            if (componente != null && !_componentes.Contains(componente))
                _componentes.Add(componente);
        }

        public void QuitarComponente(IComponentePermiso componente)
        {
            _componentes.Remove(componente);
        }

        public IReadOnlyList<IComponentePermiso> Componentes => _componentes.AsReadOnly();

        // ── Permission queries ────────────────────────────────────────────
        /// <summary>
        /// Returns the flat set of all patent IDs this role grants,
        /// traversing the full composite tree (patents + families).
        /// </summary>
        public HashSet<int> ObtenerTodasLasPatentes()
            => new HashSet<int>(_componentes.SelectMany(c => c.ObtenerPatentes()));

        /// <summary>
        /// Returns true if the role grants the specified patent.
        /// </summary>
        public bool TienePatente(int idPatente)
            => ObtenerTodasLasPatentes().Contains(idPatente);

        public override string ToString() => NombreRol;
    }
}
