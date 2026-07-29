using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class RolBE
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }

        private readonly List<IComponentePermiso> _componentes = new List<IComponentePermiso>();

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

        public HashSet<int> ObtenerTodasLasPatentes()
            => new HashSet<int>(_componentes.SelectMany(c => c.ObtenerPatentes()));

        public bool TienePatente(int idPatente)
            => ObtenerTodasLasPatentes().Contains(idPatente);

        public override string ToString() => NombreRol;
    }
}