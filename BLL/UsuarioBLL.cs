using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class UsuarioBLL
    {

        public bool CrearUsuario(UsuarioBE usuario)
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

            //Instancio DALl
            UsuarioDAL usuarios = new UsuarioDAL();

            //Llamamos al metodo de DAL y lo vinculamos con las variables de BE
            return usuarios.CrearUsuario(usuario.NombreUsuario, usuario.Contrasena);
        }

        public List<UsuarioBE> ListarUsuarios()
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ListarUsuario();
        }

        public bool DeshabilitarUsuario(int idUsuario)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.DeshabilitarUsuario(idUsuario);
        }

        public bool ModificarUsuario(UsuarioBE usuarioBE)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            return usuarioDAL.ModificarUsuario(usuarioBE);
        }
    }
}
