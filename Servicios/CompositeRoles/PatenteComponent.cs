using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class PatenteComponent : IComponentePermiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        /// <summary>
        /// A leaf simply returns its own ID.
        /// </summary>
        public IEnumerable<int> ObtenerPatentes()
        {
            yield return Id;
        }

        public override string ToString() => Nombre;
    }
}
