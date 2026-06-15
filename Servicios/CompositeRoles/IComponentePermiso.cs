using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public interface IComponentePermiso
    {
        int Id { get; }
        string Nombre { get; }
        string Descripcion { get; }

        // Returns all leaf patent IDs this component grants
        IEnumerable<int> ObtenerPatentes();
    }
}
