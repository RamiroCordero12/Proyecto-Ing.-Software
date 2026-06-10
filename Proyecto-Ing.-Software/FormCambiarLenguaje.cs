using BLL;
using Servicios;
using Servicios.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Ing._Software
{
    public partial class FormCambiarLenguaje : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormCambiarLenguaje()
        {
            InitializeComponent();
            _loc.Subscribe(this);
            ApplyLocalization();
        }

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormCambiarLenguaje", "Title"];
            lblCambiarLenguaje.Text = _loc["FormUsuarios", "LabelCambiarLenguaje"];
            btnCambiarLenguaje.Text = _loc["FormCambiarContrasena", "ButtonConfirmar"];
            int langIndex = cmbLenguaje.SelectedIndex;
            cmbLenguaje.Items.Clear();
            cmbLenguaje.Items.Add(_loc["Idiomas", "Espanol"]);
            cmbLenguaje.Items.Add(_loc["Idiomas", "Ingles"]);
            cmbLenguaje.Items.Add(_loc["Idiomas", "Portugues"]);
            // Re-select the current language so the combo stays consistent
            cmbLenguaje.SelectedIndex = (int)_loc.CurrentLanguage;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        private void btnCambiarLenguaje_Click(object sender, EventArgs e)
        {
           
                int dni = SessionManager.GetInstance.usuario.DNI;
                int lenguaje = cmbLenguaje.SelectedIndex;
                

                bool ok = new UsuarioBLL().CambiarLenguaje(dni, lenguaje);

                if (ok)
                {

                  _loc.SetLanguageByIndex(cmbLenguaje.SelectedIndex);

                }
            
            
          

        }
    }
}
