using System.Collections.Generic;

namespace BE
{
    public interface IComponentePermiso
    {
        int Id { get; }
        string Nombre { get; }
        string Descripcion { get; }
        IEnumerable<int> ObtenerPatentes();
    }
}