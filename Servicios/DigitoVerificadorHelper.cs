using System;

namespace Servicios
{
    // Calcula y valida un digito verificador simple que protege la integridad
    // de DNI + NombreUsuario contra modificaciones directas en la base de datos.
    public static class DigitoVerificadorHelper
    {
        public static int Calcular(int dni, string nombreUsuario)
        {
            string datos = dni.ToString() + (nombreUsuario ?? string.Empty);
            int suma = 0;

            for (int i = 0; i < datos.Length; i++)
            {
                suma += (i + 1) * datos[i];
            }

            return suma % 10;
        }

        public static bool Validar(Usuario usuario)
        {
            if (usuario == null) return false;
            return usuario.DigitoVerificador == Calcular(usuario.DNI, usuario.NombreUsuario);
        }
    }
}

