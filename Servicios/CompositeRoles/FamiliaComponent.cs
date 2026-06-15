using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class FamiliaComponent : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        private readonly List<IComponentePermiso> _hijos = new List<IComponentePermiso>();

        // ── Child management ──────────────────────────────────────────────
        public void Agregar(IComponentePermiso hijo)
        {
            if (hijo != null && !_hijos.Contains(hijo))
                _hijos.Add(hijo);
        }

        public void Quitar(IComponentePermiso hijo)
        {
            _hijos.Remove(hijo);
        }

        public IReadOnlyList<IComponentePermiso> Hijos => _hijos.AsReadOnly();

        // ── Composite operation ───────────────────────────────────────────
        /// <summary>
        /// Delegates to all children and merges their patent IDs (distinct).
        /// </summary>
        public IEnumerable<int> ObtenerPatentes()
            => _hijos.SelectMany(h => h.ObtenerPatentes()).Distinct();

        public override string ToString() => Nombre;
    }
}
