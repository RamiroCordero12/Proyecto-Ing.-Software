using System;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormCambiarContrasena : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormCambiarContrasena()
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
            this.Text = _loc["FormCambiarContrasena", "Title"];
            lblContraseñaActual.Text = _loc["FormCambiarContrasena", "LabelActual"];
            lblConfirmar.Text = _loc["FormCambiarContrasena", "LabelNueva"];
            lblConfirmarContraseña.Text = _loc["FormCambiarContrasena", "LabelConfirmar"];
            btnCambiarContrasena.Text = _loc["FormCambiarContrasena", "ButtonConfirmar"];
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Confirm button
        // ─────────────────────────────────────────

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            try
            {
                int dni = SessionManager.GetInstance.usuario.DNI;
                string actual = txtContrasenaActual.Text;
                string nueva = txtContrasenaNueva.Text;
                string confirmar = txtContrasenaConfirmar.Text;

                bool ok = new UsuarioBLL().CambiarContrasena(dni, actual, nueva, confirmar);

                if (ok)
                {
                    MessageBox.Show(
                        _loc["FormCambiarContrasena", "MsgExito"],
                        _loc["FormCambiarContrasena", "MsgExitoTitle"],
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtContrasenaActual.Clear();
                    txtContrasenaNueva.Clear();
                    txtContrasenaConfirmar.Clear();
                }
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message,
                    _loc["FormCambiarContrasena", "MsgValidacionTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message,
                    _loc["FormCambiarContrasena", "MsgAccesoDenegadoTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _loc["FormCambiarContrasena", "MsgErrorBase"] + ex.Message,
                    _loc["FormCambiarContrasena", "MsgErrorTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}