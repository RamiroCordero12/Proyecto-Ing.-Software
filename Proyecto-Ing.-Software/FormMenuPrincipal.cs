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

            bool esAdmin = (usuarioLogueado.Rol == 1);
            usuarioToolStripMenuItem.Visible = esAdmin;
            bitacoraToolStripMenuItem.Visible = esAdmin;
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

        private void pictureBox1_Click(object sender, EventArgs e) { }

        // Designer-generated empty handler kept for compatibility
        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }

        private void cambiarLenguajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCambiarLenguaje().Show();
        }
    }
}