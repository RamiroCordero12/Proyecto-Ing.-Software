using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormUsuarios : Form, ILocalizationObserver
    {
        private int _dniUsuario = 0;
        private int _conteoUsuarios = 0;
        private List<RolBE> _todosLosRoles = new List<RolBE>();
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormUsuarios()
        {
            InitializeComponent();

            if (SessionManager.GetInstance.Permisos != null &&
                !SessionManager.GetInstance.Permisos.Tiene(Patente.GestorUsuarios))
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgAccesoDenegado"],
                    _loc["FormUsuarios", "MsgAccesoDenegadoTitle"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            _loc.Subscribe(this);
            ApplyLocalization();
            ActualizarGrilla();
        }

        // Refresh whenever the window regains focus, so roles created in
        // FormRoles while this window stayed open (non-modal) show up here
        // without needing to close and reopen the form.
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
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
            btnRegenerarDigito.Text = _loc["FormUsuarios", "ButtonRegenerarDigito"];

            AplicarEncabezadosGrilla();

            // Refresh language combo items (keep current selection)
            int langIndex = cmbLenguaje.SelectedIndex;
            cmbLenguaje.Items.Clear();
            cmbLenguaje.Items.Add(_loc["Idiomas", "Espanol"]);
            cmbLenguaje.Items.Add(_loc["Idiomas", "Ingles"]);
            cmbLenguaje.Items.Add(_loc["Idiomas", "Portugues"]);
            cmbLenguaje.SelectedIndex = (int)_loc.CurrentLanguage;

            lblFiltroEstado.Text = _loc["FormUsuarios", "LabelFiltroEstado"];

            // Refresh filter combo items (keep current selection; index meaning
            // — 0 = Todos, 1 = Habilitados — stays fixed across language switches)
            int filtroIndex = cmbFiltroEstado.SelectedIndex;
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add(_loc["FormUsuarios", "FiltroTodos"]);
            cmbFiltroEstado.Items.Add(_loc["FormUsuarios", "FiltroHabilitados"]);
            cmbFiltroEstado.SelectedIndex = filtroIndex >= 0 ? filtroIndex : 0;

            ActualizarLabelConteo();
        }

        // Reflects whatever is currently shown in the grid (i.e. respects the
        // Todos/Habilitados filter), not the unfiltered total in the database.
        private void ActualizarLabelConteo()
        {
            lblTotalUsuarios.Text = _loc["FormUsuarios", "LabelTotalUsuarios"] + " " + _conteoUsuarios;
        }

        // Column names stay fixed (used by Cells["..."] lookups elsewhere);
        // only the displayed HeaderText is localized.
        private void AplicarEncabezadosGrilla()
        {
            if (!dgvUsuario.Columns.Contains("DNI")) return;

            dgvUsuario.Columns["DNI"].HeaderText = _loc["FormUsuarios", "ColDNI"];
            dgvUsuario.Columns["Nombre"].HeaderText = _loc["FormUsuarios", "ColNombre"];
            dgvUsuario.Columns["Apellido"].HeaderText = _loc["FormUsuarios", "ColApellido"];
            dgvUsuario.Columns["Email"].HeaderText = _loc["FormUsuarios", "ColEmail"];
            dgvUsuario.Columns["Rol"].HeaderText = _loc["FormUsuarios", "ColRol"];
            dgvUsuario.Columns["Estado"].HeaderText = _loc["FormUsuarios", "ColEstado"];
            dgvUsuario.Columns["Lenguaje"].HeaderText = _loc["FormUsuarios", "ColLenguaje"];
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
        //  Roles combo: populated dynamically from DB
        // ─────────────────────────────────────────

        private void CargarRoles()
        {
            try
            {
                _todosLosRoles = new RolesBLL().ListarRoles();

                cmbRoles.DisplayMember = "NombreRol";
                cmbRoles.ValueMember = "IdRol";
                cmbRoles.DataSource = _todosLosRoles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgErrorCargarRoles"] + ex.Message,
                    _loc["FormUsuarios", "MsgErrorGrilla"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ─────────────────────────────────────────
        //  CRUD handlers
        // ─────────────────────────────────────────

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRoles.SelectedValue == null)
                {
                    MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                        _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Usuario usuario = new Usuario
                {
                    DNI = int.Parse(txtDNI.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtEmail.Text,
                    IdRol = Convert.ToInt32(cmbRoles.SelectedValue),
                    Estado = true
                };

                int adminDNI = SessionManager.GetInstance.usuario.DNI;
                bool exito = new UsuarioBLL().CrearUsuario(usuario, adminDNI);

                if (exito)
                {
                    MessageBox.Show(_loc["FormUsuarios", "MsgUsuarioCreado"],
                        _loc["FormUsuarios", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // Reload roles first so newly-created roles (and their names)
                // are reflected both in the grid and in cmbRoles.
                CargarRoles();

                var usuarios = new UsuarioBLL().ListarUsuarios();

                // FiltroEstado: 0 = Todos, 1 = Habilitados only.
                if (cmbFiltroEstado.SelectedIndex == 1)
                    usuarios = usuarios.Where(u => u.Estado).ToList();

                // Names for each Lenguaje index, in the same order as cmbLenguaje's items.
                string[] nombresLenguaje =
                {
                    _loc["Idiomas", "Espanol"],
                    _loc["Idiomas", "Ingles"],
                    _loc["Idiomas", "Portugues"]
                };

                // Project to a display-friendly table so we show the role NAME
                // and language NAME, not just their numeric IDs.
                var display = new System.Data.DataTable();
                display.Columns.Add("DNI", typeof(int));
                display.Columns.Add("Nombre", typeof(string));
                display.Columns.Add("Apellido", typeof(string));
                display.Columns.Add("Email", typeof(string));
                display.Columns.Add("Rol", typeof(string));
                display.Columns.Add("Estado", typeof(bool));
                display.Columns.Add("Lenguaje", typeof(string));
                display.Columns.Add("LenguajeId", typeof(int));

                foreach (var u in usuarios)
                {
                    string nombreRol = _todosLosRoles
                        .FirstOrDefault(r => r.IdRol == u.IdRol)?.NombreRol
                        ?? u.IdRol.ToString();

                    string nombreLenguaje = u.Lenguaje >= 0 && u.Lenguaje < nombresLenguaje.Length
                        ? nombresLenguaje[u.Lenguaje]
                        : u.Lenguaje.ToString();

                    display.Rows.Add(u.DNI, u.Nombre, u.Apellido, u.Email, nombreRol, u.Estado, nombreLenguaje, u.Lenguaje);
                }

                dgvUsuario.DataSource = null;
                dgvUsuario.DataSource = display;

                if (dgvUsuario.Columns.Contains("LenguajeId"))
                    dgvUsuario.Columns["LenguajeId"].Visible = false;

                AplicarEncabezadosGrilla();

                _conteoUsuarios = display.Rows.Count;
                ActualizarLabelConteo();

                // Binding a new DataSource auto-selects the first row, which looks
                // like a real selection but isn't (CellClick never fired for it).
                // Clear it so a single click on the intended row is enough.
                dgvUsuario.ClearSelection();
                dgvUsuario.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgErrorGrilla"] + ex.Message,
                    _loc["FormUsuarios", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeshabilitarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuario.SelectedRows.Count == 0)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                    _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int dni = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);
            bool ok = new UsuarioBLL().DeshabilitarUsuario(dni);

            MessageBox.Show(
                ok ? _loc["FormUsuarios", "MsgUsuarioDeshabilitado"] : _loc["FormUsuarios", "MsgErrorDeshabilitar"],
                ok ? _loc["FormUsuarios", "TitleExito"] : _loc["FormUsuarios", "TitleError"],
                MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            ActualizarGrilla();
        }

        private void btnHabilitarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuario.SelectedRows.Count == 0)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                    _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int dni = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);
            bool ok = new UsuarioBLL().HabilitarUsuario(dni);

            MessageBox.Show(
                ok ? _loc["FormUsuarios", "MsgUsuarioHabilitado"] : _loc["FormUsuarios", "MsgErrorHabilitar"],
                ok ? _loc["FormUsuarios", "TitleExito"] : _loc["FormUsuarios", "TitleError"],
                MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            ActualizarGrilla();
        }

        // Recovery tool: recalculates and persists the DigitoVerificador for
        // the selected user, restoring their ability to log in after a
        // direct database edit (e.g. changed NombreUsuario or DNI by hand)
        // broke the integrity check.
        private void btnRegenerarDigito_Click(object sender, EventArgs e)
        {
            if (_dniUsuario == 0)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                    _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                _loc["FormUsuarios", "MsgConfirmRegenerarDigito"],
                _loc["FormUsuarios", "MsgConfirmRegenerarDigitoTitle"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                bool ok = new UsuarioBLL().RegenerarDigitoVerificador(_dniUsuario);

                MessageBox.Show(
                    ok ? _loc["FormUsuarios", "MsgDigitoRegenerado"] : _loc["FormUsuarios", "MsgErrorRegenerarDigito"],
                    ok ? _loc["FormUsuarios", "TitleExito"] : _loc["FormUsuarios", "TitleError"],
                    MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                ActualizarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormUsuarios", "MsgErrorRegenerarDigito"],
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            if (_dniUsuario == 0)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                    _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRoles.SelectedValue == null)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgSeleccionarUsuario"],
                    _loc["FormUsuarios", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario usuario = new Usuario
            {
                DNI = int.Parse(txtDNI.Text),
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                IdRol = Convert.ToInt32(cmbRoles.SelectedValue),
                Lenguaje = cmbLenguaje.SelectedIndex
            };

            bool exito = new UsuarioBLL().ModificarUsuario(usuario, _dniUsuario);

            if (exito)
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgUsuarioModificado"],
                    _loc["FormUsuarios", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarGrilla();
            }
            else
            {
                MessageBox.Show(_loc["FormUsuarios", "MsgErrorModificar"],
                    _loc["FormUsuarios", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string nombreRolFila = fila.Cells["Rol"].Value?.ToString();
            var rolEncontrado = _todosLosRoles.FirstOrDefault(r => r.NombreRol == nombreRolFila);
            if (rolEncontrado != null)
                cmbRoles.SelectedValue = rolEncontrado.IdRol;

            int lenguaje = Convert.ToInt32(fila.Cells["LenguajeId"].Value);
            if (lenguaje >= 0 && lenguaje < cmbLenguaje.Items.Count)
                cmbLenguaje.SelectedIndex = lenguaje;
        }

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e) { }

        private void cmbFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void FormUsuarios_Load(object sender, EventArgs e) { }
    }
}