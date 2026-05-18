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
            //Validaciones
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Apellido) || usuario.DNI == null)
            {
                throw new Exception("Completa el campo faltante");
            }
            usuario.NombreUsuario = usuario.Nombre + usuario.DNI;
            string contrasenaNueva = usuario.Apellido + usuario.DNI;
            usuario.Contrasena = Encriptador.Encriptacion(contrasenaNueva);            
            //Instancio DALl
            UsuarioDAL usuarios = new UsuarioDAL();

            //Llamamos al metodo de DAL y lo vinculamos con las variables de BE
            bool verificar =  usuarios.CrearUsuario(usuario);

            if (verificar)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuario.DNI, 
                    _bitacora.Accion = "Creación de usuario" , DateTime.Now, "Usuarios", _bitacora.Criticidad = "Alta");            
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
            if(dniUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.DeshabilitarUsuario(dniUsuario);
        }

        public bool HabilitarUsuario(int dniUsuario)
        {
            if (dniUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.HabilitarUsuario(dniUsuario);
        }

        public bool ModificarUsuario(Usuario usuario, int dniViejo)
        {
            //Evaluamos el campo que vamos a encriptar
            //if (!string.IsNullOrEmpty(usuario.Contrasena))
            //{
            //    usuario.Contrasena = Encriptador.Encriptacion(usuario.Contrasena);
            //}
            //else
            //{
            //    throw new Exception("Error al modificar la contrasena");
            //}
            //Instanciamos DAL y ejecutamos la consulta (UPDATE)
            usuario.NombreUsuario = usuario.Nombre + usuario.DNI;
            string contrasenaNueva = usuario.Apellido + usuario.DNI;
            usuario.Contrasena = Encriptador.Encriptacion(contrasenaNueva);

            UsuarioDAL usuarioDAL = new UsuarioDAL();

            bool exito = usuarioDAL.ModificarUsuario(usuario, dniViejo);

            //Registramos en la bitacora
            if (exito)
            {
                Bitacora _bitacora = new Bitacora();

                bitacora.RegistroBitacora(_bitacora.IdBitacora, usuario.DNI,
                    _bitacora.Accion = "Modificacion de usuario", DateTime.Now, "Usuarios", _bitacora.Criticidad = "Alta");
                return true;
            }
            else
            {
                throw new Exception("No se pudo modificar el usuario");
            }

        }

        public Usuario Login(string NombreUsuario, string Contrasena)
        {
            if(string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contrasena))
            {
                throw new Exception("Completa todos los campos");
            }

            string contrasenaEncriptada = Encriptador.Encriptacion(Contrasena);

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            Usuario usuarioEncontrado = usuarioDAL.Login(NombreUsuario, contrasenaEncriptada);

            if(usuarioEncontrado != null)
            {
                SessionManager.GetInstance.Login(usuarioEncontrado);
                return usuarioEncontrado;
            }
            else
            {
                throw new Exception("Error. Usuario no encontrado");
            }
        }
    }
}
