using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormUsuarios : Form, ILocalizationObserver
    {
        private int _dniUsuario = 0;
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormUsuarios()
        {
            InitializeComponent();
            _loc.Subscribe(this);
            ApplyLocalization();
            ActualizarGrilla();
        }

        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormUsuarios", "Title"];
            lblDNI.Text = _loc["FormUsuarios", "LabelDNI"];
            lblNombre.Text = _loc["FormUsuarios", "LabelNombre"];
            lblApellido.Text = _loc["FormUsuarios", "LabelApellido"];
            lblEmail.Text = _loc["FormUsuarios", "LabelEmail"];
            lblCambiarRol.Text = _loc["FormUsuarios", "LabelCambiarRol"];
            lblCambiarLenguaje.Text = _loc["FormUsuarios", "LabelCambiarLenguaje"];
            btnCrearUsuario.Text = _loc["FormUsuarios", "ButtonCrear"];
            btnDeshabilitarUsuario.Text = _loc["FormUsuarios", "ButtonDeshabilitar"];
            btnHabilitarUsuario.Text = _loc["FormUsuarios", "ButtonHabilitar"];
            btnModificarUsuario.Text = _loc["FormUsuarios", "ButtonModificar"];
            label5.Text = _loc["FormUsuarios", "LegendaRol1"];
            label6.Text = _loc["FormUsuarios", "LegendaRol2"];

            // Refresh role combo items in the active language
            int rolIndex = cmbRoles.SelectedIndex;
            cmbRoles.Items.Clear();
            cmbRoles.Items.Add(_loc["FormUsuarios", "RolAdministrador"]);
            cmbRoles.Items.Add(_loc["FormUsuarios", "RolEmpleado"]);
            if (rolIndex >= 0 && rolIndex < cmbRoles.Items.Count)
                cmbRoles.SelectedIndex = rolIndex;

            // Refresh language combo items (keep current selection)
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

        // ─────────────────────────────────────────
        //  Language combo
        // ─────────────────────────────────────────

      //  private void cmbLenguaje_SelectedIndexChanged(object sender, EventArgs e)
        //{
            // Guard: only react to real user interaction, not programmatic refresh
          //  if (cmbLenguaje.SelectedIndex == (int)_loc.CurrentLanguage) return;
            //_loc.SetLanguageByIndex(cmbLenguaje.SelectedIndex);

            // Persist the choice to the logged-in user's profile (DAL not shown
            // here, but this is the right place to call it)
            // e.g. new UsuarioBLL().CambiarLenguaje(SessionManager.GetInstance.usuario.DNI, newLang);
//        }

        // ─────────────────────────────────────────
        //  CRUD handlers
        // ─────────────────────────────────────────

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    DNI = int.Parse(txtDNI.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtEmail.Text,
                    Rol = cmbRoles.SelectedIndex + 1,
                    Estado = true
                };

                int adminDNI = SessionManager.GetInstance.usuario.DNI;
                bool exito = new UsuarioBLL().CrearUsuario(usuario, adminDNI);

                if (exito)
                {
                    MessageBox.Show(_loc["FormUsuarios", "MsgUsuarioCreado"]);
                    ActualizarGrilla();
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                string msg = (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    ? _loc["FormUsuarios", "MsgErrorDuplicado"]
                    : _loc["FormUsuarios", "MsgErrorBD"];

                MessageBox.Show(msg,
                    _loc["FormUsuarios", "MsgErrorCrear"],
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    _loc["FormUsuarios", "MsgErrorCrear"],
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarGrilla()
        {
            try
            {
                dgvUsuario.DataSource = null;
                dgvUsuario.DataSource = new UsuarioBLL().ListarUsuarios();
                if (dgvUsuario.Columns.Contains("Contrasena"))
                    dgvUsuario.Columns["Contrasena"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgErrorGrilla"] + ex.Message);
            }
        }

        private void btnDeshabilitarUsuario_Click(object sender, EventArgs e)
        {
            int dni = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);
            bool ok = new UsuarioBLL().DeshabilitarUsuario(dni);

            MessageBox.Show(ok
                ? _loc["FormUsuarios", "MsgUsuarioDeshabilitado"]
                : _loc["FormUsuarios", "MsgErrorDeshabilitar"]);

            ActualizarGrilla();
        }

        private void btnHabilitarUsuario_Click(object sender, EventArgs e)
        {
            int dni = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);
            bool ok = new UsuarioBLL().HabilitarUsuario(dni);

            MessageBox.Show(ok
                ? _loc["FormUsuarios", "MsgUsuarioHabilitado"]
                : _loc["FormUsuarios", "MsgErrorHabilitar"]);

            ActualizarGrilla();
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            if (_dniUsuario == 0)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"]);
                return;
            }

            Usuario usuario = new Usuario
            {
                DNI = int.Parse(txtDNI.Text),
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                Rol = cmbRoles.SelectedIndex + 1,
                Lenguaje = cmbLenguaje.SelectedIndex
            };

            bool exito = new UsuarioBLL().ModificarUsuario(usuario, _dniUsuario);

            if (exito)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgUsuarioModificado"]);
                ActualizarGrilla();
               

            }
            else
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgErrorModificar"]);
            }
        }

        private void dgvUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvUsuario.Rows[e.RowIndex];
            _dniUsuario = Convert.ToInt32(fila.Cells["DNI"].Value);
            txtDNI.Text = fila.Cells["DNI"].Value.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
            txtEmail.Text = fila.Cells["Email"].Value.ToString();
        }

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e) { }

        private void FormUsuarios_Load(object sender, EventArgs e) { }
    }
}