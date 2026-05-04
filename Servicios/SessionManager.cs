using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SessionManager
    {
        public static SessionManager _instancia;

        public Usuario usuario { get; private set; }

        private SessionManager()
        {

        }

        //Singleton
        public static SessionManager GetInstance
        {
            get
            {
                if(_instancia == null)
                {
                    _instancia = new SessionManager();
                }

                return _instancia;
            }
        }

        public void Login(Usuario UsuarioLogueado)
        {
            usuario = UsuarioLogueado;
        }

        public void Logout()
        {
            usuario = null;
        }
    }
}
