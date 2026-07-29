using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class FamiliaComponent : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        private readonly List<IComponentePermiso> _hijos = new List<IComponentePermiso>();

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

        public IEnumerable<int> ObtenerPatentes()
            => _hijos.SelectMany(h => h.ObtenerPatentes()).Distinct();

        public override string ToString() => Nombre;
    }
}