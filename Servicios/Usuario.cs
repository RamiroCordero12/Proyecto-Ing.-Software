using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Usuario
    {
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public bool Estado { get; set; }
        public string Email { get; set; }

        [Obsolete("Use IdRol with the Composite permission system instead.")]
        public int Rol { get; set; }

        // New: FK to Roles (composite pattern) table.
        public int IdRol { get; set; }

        // nuevo
        public int IntentosFallidos { get; set; }
        public int Lenguaje { get; set; }
        public int DigitoVerificador { get; set; }
    }
}

