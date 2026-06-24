using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios.Localization;
using static Servicios.Permisos;

namespace Servicios
{
    public class SessionManager
    {
        private static volatile SessionManager _instancia;
        private static readonly object _lock = new object();

        public Usuario usuario { get; private set; }
        public Permisos Permisos { get; private set; }

        private SessionManager()
        {
        }

        //Singleton (thread-safe double-checked locking)
        public static SessionManager GetInstance
        {
            get
            {
                if (_instancia == null)
                {
                    lock (_lock)
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SessionManager();
                        }
                    }
                }

                return _instancia;
            }
        }

        public void Login(Usuario UsuarioLogueado)
        {
            usuario = UsuarioLogueado;
        }

        public void SetPermisos(Permisos permisos)
        {
            Permisos = permisos;
        }

        public void Logout()
        {
            usuario = null;
            Permisos = null;

            // The login screen must always show in the default language,
            // regardless of which language the previous user had selected.
            LocalizationService.Instance.SetLanguage(AppLanguage.Espanol);
        }
    }
}

