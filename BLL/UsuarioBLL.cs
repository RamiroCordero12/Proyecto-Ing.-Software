using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Servicios;

namespace BLL
{
    public class UsuarioBLL
    {
        BitacoraDAL bitacora = new BitacoraDAL();

        public bool CrearUsuario(Usuario usuario, int dniAdminActual)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                throw new Exception("Completa el campo faltante");
            }

            if (usuario.IdRol <= 0)
            {
                throw new Exception("Debe seleccionar un rol valido");
            }

            usuario.NombreUsuario = usuario.Nombre + usuario.DNI;
            string contrasenaNueva = usuario.Apellido + usuario.DNI;
            usuario.Contrasena = Encriptador.Encriptacion(contrasenaNueva);

            UsuarioDAL usuarios = new UsuarioDAL();

            bool verificar = usuarios.CrearUsuario(usuario);

            if (verificar)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuario.DNI,
                    _bitacora.Accion = "Creación de usuario", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
                return true;
            }
            else
            {
                throw new Exception("No se pudo crear el usuario");
            }
        }

        public List<Usuario> ListarUsuarios()
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ListarUsuario();
        }

        public bool DeshabilitarUsuario(int dniUsuario)
        {
            if (dniUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            bool verificar = usuarioDAL.DeshabilitarUsuario(dniUsuario);

            if (verificar)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, dniUsuario,
                    _bitacora.Accion = "Deshabilitar usuario", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
                return true;
            }

            return false;
        }

        public bool HabilitarUsuario(int dniUsuario)
        {
            if (dniUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            bool verificar = usuarioDAL.HabilitarUsuario(dniUsuario);

            if (verificar)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, dniUsuario,
                    _bitacora.Accion = "Habilitar usuario", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
                return true;
            }

            return false;
        }

        public bool ModificarUsuario(Usuario usuario, int dniViejo)
        {
            usuario.NombreUsuario = usuario.Nombre + usuario.DNI;

            UsuarioDAL usuarioDAL = new UsuarioDAL();

            // Preserve the existing password hash instead of resetting it on every edit.
            string hashExistente = usuarioDAL.ObtenerContrasenaHash(dniViejo);
            usuario.Contrasena = hashExistente ?? Encriptador.Encriptacion(usuario.Apellido + usuario.DNI);

            bool exito = usuarioDAL.ModificarUsuario(usuario, dniViejo);

            if (exito)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuario.DNI,
                    _bitacora.Accion = "Modificacion de usuario", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
                return true;
            }
            else
            {
                throw new Exception("No se pudo modificar el usuario");
            }
        }

        public Usuario Login(string NombreUsuario, string Contrasena)
        {
            if (string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contrasena))
                throw new Exception("Completa todos los campos");

            string contrasenaEncriptada = Encriptador.Encriptacion(Contrasena);

            UsuarioDAL usuarioDAL = new UsuarioDAL();

            Usuario usuarioEncontrado = usuarioDAL.GetUsuarioByNombreUsuario(NombreUsuario);

            if (usuarioEncontrado == null)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }

            if (!DigitoVerificadorHelper.Validar(usuarioEncontrado))
            {
                Bitacora _bitacoraIntegridad = new Bitacora();
                bitacora.RegistroBitacora(_bitacoraIntegridad.IdBitacora, usuarioEncontrado.DNI,
                    _bitacoraIntegridad.Accion = "Digito verificador invalido detectado", DateTime.Now,
                    "FormLogin", _bitacoraIntegridad.Criticidad = "Alta");

                throw new Exception("Los datos del usuario fueron alterados. Contacte al administrador.");
            }

            if (!usuarioEncontrado.Estado)
            {
                throw new Exception("Cuenta bloqueada. Contacte al administrador.");
            }

            if (usuarioEncontrado.Contrasena == contrasenaEncriptada)
            {
                if (usuarioEncontrado.IntentosFallidos != 0)
                {
                    usuarioDAL.ActualizarIntentosYEstado(usuarioEncontrado.DNI, 0, true);
                }

                Bitacora _bitacora = new Bitacora();
                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuarioEncontrado.DNI,
                    _bitacora.Accion = "Login de usuario", DateTime.Now,
                    "FormLogin", _bitacora.Criticidad = "Alta");

                SessionManager.GetInstance.Login(usuarioEncontrado);

                // Load and cache this user's permission tree for the session.
                var rolCompleto = new RolesBLL().ObtenerRolConPermisos(usuarioEncontrado.IdRol);
                SessionManager.GetInstance.SetPermisos(new Permisos(rolCompleto));

                return usuarioEncontrado;
            }
            else
            {
                int nuevosIntentos = usuarioEncontrado.IntentosFallidos + 1;
                bool bloquear = nuevosIntentos >= 3;

                usuarioDAL.ActualizarIntentosYEstado(usuarioEncontrado.DNI, nuevosIntentos, bloquear ? false : true);

                Bitacora _bitacora = new Bitacora();
                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuarioEncontrado.DNI,
                    _bitacora.Accion = $"Intento fallido #{nuevosIntentos}", DateTime.Now,
                    "FormLogin", _bitacora.Criticidad = "Media");

                if (bloquear)
                {
                    bitacora.RegistroBitacora(_bitacora.IdBitacora, usuarioEncontrado.DNI,
                        _bitacora.Accion = "Cuenta deshabilitada por intentos fallidos", DateTime.Now,
                        "FormLogin", _bitacora.Criticidad = "Alta");

                    throw new Exception("Cuenta bloqueada. Contacte al administrador.");
                }
                else
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }
            }
        }

        public bool CambiarContrasena(int dni, string contrasenaActualPlain, string contrasenaNuevaPlain, string contrasenaConfirmPlain)
        {
            if (string.IsNullOrWhiteSpace(contrasenaNuevaPlain) || contrasenaNuevaPlain.Length < 8)
                throw new ArgumentException("La nueva contraseña debe tener al menos 8 caracteres.");

            if (contrasenaNuevaPlain != contrasenaConfirmPlain)
                throw new ArgumentException("La nueva contraseña y la confirmación no coinciden.");

            UsuarioDAL dal = new UsuarioDAL();
            string hashActualEnDb = dal.ObtenerContrasenaHash(dni);
            if (hashActualEnDb == null) throw new Exception("Usuario no encontrado.");

            string providedActualHash = Encriptador.Encriptacion(contrasenaActualPlain);
            if (providedActualHash != hashActualEnDb)
                throw new UnauthorizedAccessException("La contraseña actual es incorrecta.");

            string nuevaHash = Encriptador.Encriptacion(contrasenaNuevaPlain);
            bool exito = dal.CambiarContrasena(dni, nuevaHash);
            if (exito)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, dni,
                    _bitacora.Accion = "Cambio de contraseña", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
            }
            return exito;
        }

        public bool CambiarLenguaje(int dniUsuario, int lenguaje)
        {
            if (dniUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            bool verificar = usuarioDAL.CambiarLenguaje(dniUsuario, lenguaje);

            if (verificar)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, dniUsuario,
                    _bitacora.Accion = "Lenguaje cambiado", DateTime.Now, "Gestor de usuarios", _bitacora.Criticidad = "Alta");
            }

            return verificar;
        }
    }
}

