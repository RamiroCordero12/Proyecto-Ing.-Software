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

        public bool CrearUsuario(Usuario usuario)
        {
            //Validaciones
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                throw new Exception("Completa el campo de nombre de usuario");
            }
            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
            {
                throw new Exception("Completa el campo de la contrasena");
            }

            //Encriptamos la contrasena del usuario creado
            usuario.Contrasena = Encriptador.Encriptacion(usuario.Contrasena);
            
            //Instancio DALl
            UsuarioDAL usuarios = new UsuarioDAL();

            //Llamamos al metodo de DAL y lo vinculamos con las variables de BE
            return usuarios.CrearUsuario(usuario);
        }

        public List<Usuario> ListarUsuarios()
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ListarUsuario();
        }

        public bool DeshabilitarUsuario(int idUsuario)
        {
            if(idUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.DeshabilitarUsuario(idUsuario);
        }

        public bool HabilitarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new Exception("Error al seleccionar un usuario");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.HabilitarUsuario(idUsuario);
        }

        public bool ModificarUsuario(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.NombreUsuario))
            {
                usuario.Contrasena = Encriptador.Encriptacion(usuario.Contrasena);
            }
            else
            {
                throw new Exception("Error al modificar la contrasena");
            }

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ModificarUsuario(usuario);
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
