using System;
using System.Windows.Forms;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormMenuPrincipal : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormMenuPrincipal()
        {
            InitializeComponent();
            _loc.Subscribe(this);
            ApplyLocalization();
        }

        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormMenuPrincipal", "Title"];
            usuarioToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuUsuarios"];
            logoutToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuLogout"];
            bitacoraToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuBitacora"];
            cambiarLenguajeToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuCambiarLenguaje"];
            cambiarContrasenaToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuCambiarContrasena"];
            reloginToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuRelogin"];
            familiasToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuFamilias"];
            rolesToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuRoles"];
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Load: enforce role-based visibility
        // ─────────────────────────────────────────

        private void Form1_Load(object sender, EventArgs e)
        {
            Usuario usuarioLogueado = SessionManager.GetInstance.usuario;
            Permisos permisos = SessionManager.GetInstance.Permisos;

            // Defensive fallback: if permissions weren't loaded for some reason
            // (e.g. relogin path), reload them now instead of crashing or
            // silently hiding everything.
            if (permisos == null)
            {
                var rolCompleto = new BLL.RolesBLL().ObtenerRolConPermisos(usuarioLogueado.IdRol);
                permisos = new Permisos(rolCompleto);
                SessionManager.GetInstance.SetPermisos(permisos);
            }

            usuarioToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);
            bitacoraToolStripMenuItem.Visible = permisos.Tiene(Patente.Bitacora);
            cambiarContrasenaToolStripMenuItem.Visible = permisos.Tiene(Patente.CambiarContrasena);
            reloginToolStripMenuItem.Visible = permisos.Tiene(Patente.ReiniciarSesion);
            logoutToolStripMenuItem.Visible = permisos.Tiene(Patente.CerrarSesion);

            // Familias/Roles management is part of the same "Gestor de usuarios" patent family
            // (administering the permission system itself) — gate it the same way.
            familiasToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);
            rolesToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);
        }

        // ─────────────────────────────────────────
        //  Menu handlers
        // ─────────────────────────────────────────

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormUsuarios().Show();
        }

        private void bitacoraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new FormBitacora().Show();
        }

        private void cambiarContrasenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCambiarContrasena().Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                _loc["FormMenuPrincipal", "LogoutConfirm"],
                _loc["FormMenuPrincipal", "LogoutTitle"],
                MessageBoxButtons.YesNo);

            if (confirmar != DialogResult.Yes) return;

            this.Hide();
            SessionManager.GetInstance.Logout();

            using (var login = new FormLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }
            this.Show();
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show(
                _loc["FormMenuPrincipal", "ReloginConfirm"],
                _loc["FormMenuPrincipal", "ReloginTitle"],
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resp != DialogResult.Yes) return;

            SessionManager.GetInstance.Logout();
            this.Hide();

            using (var login = new FormLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }
            this.Show();
        }

        private void cambiarLenguajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCambiarLenguaje().Show();
        }

        private void familiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormFamilias().Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormRoles().Show();
        }
    }
}