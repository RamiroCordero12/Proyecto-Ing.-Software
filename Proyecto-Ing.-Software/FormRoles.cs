using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormRoles : Form, ILocalizationObserver
    {
        private int _idRolSeleccionado = 0;
        private List<PatenteComponent> _todasLasPatentes = new List<PatenteComponent>();
        private List<FamiliaComponent> _todasLasFamilias = new List<FamiliaComponent>();
        private List<RolBE> _todosLosRoles = new List<RolBE>();
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormRoles()
        {
            InitializeComponent();

            if (SessionManager.GetInstance.Permisos != null &&
                !SessionManager.GetInstance.Permisos.Tiene(Patente.GestorUsuarios))
            {
                MessageBox.Show(_loc["FormRoles", "MsgAccesoDenegado"],
                    _loc["FormRoles", "MsgAccesoDenegadoTitle"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            _loc.Subscribe(this);
            ApplyLocalization();
            CargarPatentes();
            CargarFamilias();
            ActualizarGrilla();
        }

        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormRoles", "Title"];
            label6.Text = _loc["FormRoles", "Title"];
            label1.Text = _loc["FormRoles", "LabelNombreRol"];
            label2.Text = _loc["FormRoles", "LabelDescripcion"];
            label3.Text = _loc["FormRoles", "LabelPatentes"];
            label4.Text = _loc["FormRoles", "LabelFamilias"];
            label5.Text = _loc["FormRoles", "LabelExistentes"];
            btnCrearRol.Text = _loc["FormRoles", "ButtonCrear"];
            btnModificar.Text = _loc["FormRoles", "ButtonModificar"];
            btnEliminar.Text = _loc["FormRoles", "ButtonEliminar"];
            btnLimpiar.Text = _loc["FormRoles", "ButtonLimpiar"];

            AplicarEncabezadosGrilla();
            RefrescarTextosPatentes();
        }

        // Column names stay fixed (used by Cells["..."] lookups elsewhere);
        // only the displayed HeaderText is localized.
        private void AplicarEncabezadosGrilla()
        {
            if (!dgvRoles.Columns.Contains("ID")) return;

            dgvRoles.Columns["ID"].HeaderText = _loc["FormRoles", "ColID"];
            dgvRoles.Columns["Nombre"].HeaderText = _loc["FormRoles", "ColNombre"];
            dgvRoles.Columns["Descripcion"].HeaderText = _loc["FormRoles", "ColDescripcion"];
            dgvRoles.Columns["Permisos"].HeaderText = _loc["FormRoles", "ColPermisos"];
        }

        // Patentes are fixed system permissions (unlike Familias/Roles names,
        // which are admin-entered and must NOT be translated).
        private string NombreLocalizadoPatente(int idPatente) => _loc["Patentes", idPatente.ToString()];

        // Re-label existing clbPatentes items in place (by index) so a language
        // switch doesn't reset the user's current checkbox selections.
        private void RefrescarTextosPatentes()
        {
            for (int i = 0; i < clbPatentes.Items.Count; i++)
            {
                var item = (PatenteItem)clbPatentes.Items[i];
                clbPatentes.Items[i] = new PatenteItem(item.Patente, NombreLocalizadoPatente(item.Patente.Id));
            }
        }

        // Wraps a PatenteComponent so the checklist displays its localized
        // name while CargarPatentes()/CellClick code can still get back to
        // the underlying entity (and its DB-stable Id) for selection logic.
        private sealed class PatenteItem
        {
            public PatenteComponent Patente { get; }
            private readonly string _texto;
            public PatenteItem(PatenteComponent patente, string texto)
            {
                Patente = patente;
                _texto = texto;
            }
            public override string ToString() => _texto;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private void CargarPatentes()
        {
            try
            {
                _todasLasPatentes = new PatentesBLL().ListarPatentes();
                clbPatentes.Items.Clear();
                foreach (var p in _todasLasPatentes)
                    clbPatentes.Items.Add(new PatenteItem(p, NombreLocalizadoPatente(p.Id)), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormRoles", "MsgErrorCargarPatentes"] + ex.Message,
                    _loc["FormRoles", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarFamilias()
        {
            try
            {
                _todasLasFamilias = new FamiliasBLL().ListarFamilias();
                clbFamilias.Items.Clear();
                foreach (var f in _todasLasFamilias)
                    clbFamilias.Items.Add(f, false);
            }
            catch (Exception ex)
            {
               MessageBox.Show(_loc["FormRoles", "MsgErrorCargarFamilias"] + ex.Message,
                                 _loc["FormRoles", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGrilla()
        {
            try
            {
                _todosLosRoles = new RolesBLL().ListarRoles();

                var display = new System.Data.DataTable();
                display.Columns.Add("ID", typeof(int));
                display.Columns.Add("Nombre", typeof(string));
                display.Columns.Add("Descripcion", typeof(string));
                display.Columns.Add("Permisos", typeof(string));

                var rolesBLL = new RolesBLL();

                foreach (var r in _todosLosRoles)
                {
                    // Load the full tree so we can show a readable summary
                    RolBE completo = rolesBLL.ObtenerRolConPermisos(r.IdRol);
                    var nombres = new List<string>();
                    foreach (var c in completo.Componentes)
                    {
                        // Patentes are fixed system permissions (localize them);
                        // Familias are admin-named (show their name as-is).
                        nombres.Add(c is PatenteComponent pc ? NombreLocalizadoPatente(pc.Id) : c.Nombre);
                    }

                    display.Rows.Add(
                        r.IdRol,
                        r.NombreRol,
                        r.Descripcion,
                        string.Join(", ", nombres));
                }

                dgvRoles.DataSource = display;

                if (dgvRoles.Columns.Contains("ID"))
                    dgvRoles.Columns["ID"].Width = 40;

                AplicarEncabezadosGrilla();

                // Binding a new DataSource auto-selects the first row, which looks
                // like a real selection but isn't (CellClick never fired for it).
                dgvRoles.ClearSelection();
                dgvRoles.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormRoles", "MsgErrorCargarRoles"] + ex.Message,
     _loc["FormRoles", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _idRolSeleccionado = 0;
            txtNombreRol.Clear();
            txtDescripcion.Clear();

            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, false);

            for (int i = 0; i < clbFamilias.Items.Count; i++)
                clbFamilias.SetItemChecked(i, false);
        }

        private List<int> ObtenerPatentesMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbPatentes.CheckedIndices)
                ids.Add(_todasLasPatentes[idx].Id);
            return ids;
        }

        private List<int> ObtenerFamiliasMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbFamilias.CheckedIndices)
                ids.Add(_todasLasFamilias[idx].Id);
            return ids;
        }

        // ── Grid click: load role into form ───────────────────────────────
        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvRoles.Rows[e.RowIndex];
            object idValue = row.Cells["ID"].Value;
            if (idValue == null || idValue == DBNull.Value) return;

            _idRolSeleccionado = Convert.ToInt32(idValue);

            RolBE rol = _todosLosRoles.Find(r => r.IdRol == _idRolSeleccionado);
            if (rol == null) return;

            txtNombreRol.Text = rol.NombreRol;
            txtDescripcion.Text = rol.Descripcion;

            try
            {
                var rolesBLL = new RolesBLL();
                List<int> idsPatentesDirectas = rolesBLL.ObtenerIdPatentesPorRol(_idRolSeleccionado);
                List<int> idsFamilias = rolesBLL.ObtenerIdFamiliasPorRol(_idRolSeleccionado);

                var setPatentes = new HashSet<int>(idsPatentesDirectas);
                var setFamilias = new HashSet<int>(idsFamilias);

                for (int i = 0; i < clbPatentes.Items.Count; i++)
                {
                    var item = (PatenteItem)clbPatentes.Items[i];
                    clbPatentes.SetItemChecked(i, setPatentes.Contains(item.Patente.Id));
                }

                for (int i = 0; i < clbFamilias.Items.Count; i++)
                {
                    var f = (FamiliaComponent)clbFamilias.Items[i];
                    clbFamilias.SetItemChecked(i, setFamilias.Contains(f.Id));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormRoles", "MsgErrorCargarPermisos"] + ex.Message,
    _loc["FormRoles", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (_idRolSeleccionado == 0)
            {
                MessageBox.Show(_loc["FormRoles", "MsgSeleccionarEliminar"],
     _loc["FormRoles", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Block deleting a role that's still in use by some user would require
            // a check against Usuarios; the FK constraint will raise a SqlException
            // if any user still references this role, which we surface below.
            DialogResult confirm = MessageBox.Show(
                _loc["FormRoles", "MsgConfirmEliminar"],
                _loc["FormRoles", "MsgConfirmEliminarTitle"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = new RolesBLL().EliminarRol(_idRolSeleccionado);

                if (ok)
                {
                    MessageBox.Show(_loc["FormRoles", "MsgRolEliminado"],
    _loc["FormRoles", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show(
                                        _loc["FormRoles", "MsgErrorRolEnUso"],
                    _loc["FormRoles", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormRoles", "MsgErrorEliminarTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrearRol_Click(object sender, EventArgs e)
        {
            try
            {
                var rol = new RolBE
                {
                    NombreRol = txtNombreRol.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();
                List<int> idsFamilias = ObtenerFamiliasMarcadas();

                bool ok = new RolesBLL().CrearRol(rol, idsPatentes, idsFamilias);

                if (ok)
                {
                    MessageBox.Show(_loc["FormRoles", "MsgRolCreado"],
                        _loc["FormRoles", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormRoles", "MsgErrorCrearTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (_idRolSeleccionado == 0)
            {
                MessageBox.Show(_loc["FormRoles", "MsgSeleccionarModificar"],
    _loc["FormRoles", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var rol = new RolBE
                {
                    IdRol = _idRolSeleccionado,
                    NombreRol = txtNombreRol.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();
                List<int> idsFamilias = ObtenerFamiliasMarcadas();

                bool ok = new RolesBLL().ModificarRol(rol, idsPatentes, idsFamilias);

                if (ok)
                {
                    MessageBox.Show(_loc["FormRoles", "MsgRolModificado"],
    _loc["FormRoles", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormRoles", "MsgErrorModificarTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
