using System.Collections.Generic;

namespace BE
{
    public class PatenteComponent : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public IEnumerable<int> ObtenerPatentes()
        {
            yield return Id;
        }

        public override string ToString() => Nombre;
    }
}