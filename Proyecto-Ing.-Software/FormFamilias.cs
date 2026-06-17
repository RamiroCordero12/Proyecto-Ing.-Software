using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormFamilias : Form, ILocalizationObserver
    {
        private int _idFamiliaSeleccionada = 0;
        private List<PatenteComponent> _todasLasPatentes = new List<PatenteComponent>();
        private List<FamiliaComponent> _todasLasFamilias = new List<FamiliaComponent>();
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormFamilias()
        {
            InitializeComponent();
            _loc.Subscribe(this);
            ApplyLocalization();
            CargarPatentes();
            ActualizarGrilla();
        }
        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormFamilias", "Title"];
            label1.Text = _loc["FormFamilias", "Title"];
            label2.Text = _loc["FormFamilias", "LabelNombre"];
            label3.Text = _loc["FormFamilias", "LabelDescripcion"];
            label4.Text = _loc["FormFamilias", "LabelPatentes"];
            label5.Text = _loc["FormFamilias", "LabelExistentes"];
            btnCrearFamilia.Text = _loc["FormFamilias", "ButtonCrear"];
            btnModificar.Text = _loc["FormFamilias", "ButtonModificar"];
            btnEliminar.Text = _loc["FormFamilias", "ButtonEliminar"];
            btnLimpiar.Text = _loc["FormFamilias", "ButtonLimpiar"];
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
                    clbPatentes.Items.Add(p, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormFamilias", "MsgErrorCargarPatentes"] + ex.Message,
    _loc["FormFamilias", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGrilla()
        {
            try
            {
                _todasLasFamilias = new FamiliasBLL().ListarFamilias();

                // Build a flat DataTable-like list for display
                var display = new System.Data.DataTable();
                display.Columns.Add("ID", typeof(int));
                display.Columns.Add("Nombre", typeof(string));
                display.Columns.Add("Descripcion", typeof(string));
                display.Columns.Add("Patentes", typeof(string));

                foreach (var f in _todasLasFamilias)
                {
                    var nombres = new List<string>();
                    foreach (var h in f.Hijos)
                        nombres.Add(h.Nombre);

                    display.Rows.Add(
                        f.Id,
                        f.Nombre,
                        f.Descripcion,
                        string.Join(", ", nombres));
                }

                dgvFamilias.DataSource = display;

                if (dgvFamilias.Columns.Contains("ID"))
                    dgvFamilias.Columns["ID"].Width = 40;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormFamilias", "MsgErrorCargarFamilias"] + ex.Message,
    _loc["FormFamilias", "TitleError"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _idFamiliaSeleccionada = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, false);
        }

        private List<int> ObtenerPatentesMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbPatentes.CheckedIndices)
                ids.Add(_todasLasPatentes[idx].Id);
            return ids;
        }

        // ── Grid click: load row into form ────────────────────────────────
        private void dgvFamilias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvFamilias.Rows[e.RowIndex];
            _idFamiliaSeleccionada = Convert.ToInt32(row.Cells["ID"].Value);

            // Find the FamiliaComponent
            FamiliaComponent fam = _todasLasFamilias.Find(f => f.Id == _idFamiliaSeleccionada);
            if (fam == null) return;

            txtNombre.Text = fam.Nombre;
            txtDescripcion.Text = fam.Descripcion;

            // Tick only the patents this family contains
            var idsEnFamilia = new HashSet<int>();
            foreach (var h in fam.Hijos)
                idsEnFamilia.Add(h.Id);

            for (int i = 0; i < clbPatentes.Items.Count; i++)
            {
                var p = (PatenteComponent)clbPatentes.Items[i];
                clbPatentes.SetItemChecked(i, idsEnFamilia.Contains(p.Id));
            }
        }
       

        

       
       

        private void btnCrearFamilia_Click(object sender, EventArgs e)
        {

            try
            {
                var familia = new FamiliaComponent
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();

                bool ok = new FamiliasBLL().CrearFamilia(familia, idsPatentes);

                if (ok)
                {
                    MessageBox.Show(_loc["FormFamilias", "MsgFamiliaCreada"],
    _loc["FormFamilias", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormFamilias", "MsgErrorCrearTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();

        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (_idFamiliaSeleccionada == 0)
            {
                MessageBox.Show(_loc["FormFamilias", "MsgSeleccionarEliminar"],
    _loc["FormFamilias", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                               _loc["FormFamilias", "MsgConfirmEliminar"],
                _loc["FormFamilias", "MsgConfirmEliminarTitle"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = new FamiliasBLL().EliminarFamilia(_idFamiliaSeleccionada);

                if (ok)
                {
                    MessageBox.Show(_loc["FormFamilias", "MsgFamiliaEliminada"],
     _loc["FormFamilias", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormFamilias", "MsgErrorEliminarTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (_idFamiliaSeleccionada == 0)
            {
                MessageBox.Show(_loc["FormFamilias", "MsgSeleccionarModificar"],
     _loc["FormFamilias", "TitleAviso"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var familia = new FamiliaComponent
                {
                    Id = _idFamiliaSeleccionada,
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();

                bool ok = new FamiliasBLL().ModificarFamilia(familia, idsPatentes);

                if (ok)
                {
                    MessageBox.Show(_loc["FormFamilias", "MsgFamiliaModificada"],
    _loc["FormFamilias", "TitleExito"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _loc["FormFamilias", "MsgErrorModificarTitle"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
