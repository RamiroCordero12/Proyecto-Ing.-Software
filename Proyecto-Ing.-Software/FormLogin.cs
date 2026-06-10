using System;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormLogin : Form, ILocalizationObserver
    {
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormLogin()
        {
            InitializeComponent();

            // Subscribe BEFORE ApplyLocalization so the form stays in sync
            // if the language is switched from another form while this one is open.
            _loc.Subscribe(this);
            ApplyLocalization();
        }

        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        // ─────────────────────────────────────────
        //  Apply strings from the active locale
        // ─────────────────────────────────────────

        private void ApplyLocalization()
        {
            // The designer already placed label3 (title), lblNombreUsuario,
            // lblContraseña and btnLogin – we just update their Text.
            label3.Text = _loc["FormLogin", "Title"];
            lblNombreUsuario.Text = _loc["FormLogin", "LabelUsername"];
            lblContraseña.Text = _loc["FormLogin", "LabelPassword"];
            btnLogin.Text = _loc["FormLogin", "ButtonLogin"];
            this.Text = _loc["FormLogin", "Title"];
        }

        // ─────────────────────────────────────────
        //  Unsubscribe when the form is disposed
        // ─────────────────────────────────────────

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Login button
        // ─────────────────────────────────────────

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuarioLogueado =
                    _usuarioBLL.Login(txtNombreUsuario.Text, txtContrasena.Text);

                _loc.SetLanguageByIndex(usuarioLogueado.Lenguaje);
                MessageBox.Show(
                    string.Format(_loc["FormLogin", "WelcomeMessage"],
                                  txtNombreUsuario.Text));

                this.DialogResult = DialogResult.OK;
                

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormLogin", "ErrorTitle"]);
            }
        }
    }
}