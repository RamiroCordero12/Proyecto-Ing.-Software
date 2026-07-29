using System;

namespace BE
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

        public int IdRol { get; set; }

        public int IntentosFallidos { get; set; }
        public int Lenguaje { get; set; }
        public int DigitoVerificador { get; set; }
    }
}